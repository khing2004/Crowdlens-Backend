using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrowdLens.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Crowdlens_backend.Helpers;

namespace Crowdlens_backend.Services
{
    /// <summary>
    /// Background service that monitors area crowd levels and sends alerts to subscribed users
    /// when their custom thresholds are exceeded
    /// </summary>
    public class CrowdAlertBackgroundService : BackgroundService
    {
        private readonly ILogger<CrowdAlertBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5); // Check every 5 minutes

        public CrowdAlertBackgroundService(ILogger<CrowdAlertBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CrowdAlertBackgroundService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndSendAlertsAsync();
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in CrowdAlertBackgroundService: {ex.Message}");
                    // Continue running even if there's an error
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }

            _logger.LogInformation("CrowdAlertBackgroundService stopped");
        }

        private async Task CheckAndSendAlertsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CrowdLensDbContext>();
                var pushNotificationService = scope.ServiceProvider.GetRequiredService<IPushNotificationService>();

                try
                {
                    // Get all active subscriptions with enabled notifications
                    var activeSubscriptions = await context.UserAreaSubscriptions
                        .Where(uas => uas.IsNotificationEnabled)
                        .Include(uas => uas.Area)
                        .Include(uas => uas.User)
                        .ToListAsync();

                    // Group subscriptions by area for efficient processing
                    var subscriptionsByArea = activeSubscriptions.GroupBy(uas => uas.AreaId);

                    foreach (var areaGroup in subscriptionsByArea)
                    {
                        var area = areaGroup.First().Area;
                        var areaOccupancyRate = area.OccupancyRate;
                        var densityLevel = CrowdDensityHelper.GetLevel(areaOccupancyRate);

                        // Find all subscriptions to this area that exceed their thresholds
                        var alertsToSend = areaGroup
                            .Where(uas => areaOccupancyRate >= uas.CrowdThresholdPercentage)
                            .ToList();

                        if (alertsToSend.Any())
                        {
                            _logger.LogInformation($"Sending alerts for area {area.AreaName} (Occupancy: {areaOccupancyRate:F1}%)");

                            foreach (var subscription in alertsToSend)
                            {
                                // Check if enough time has passed since last alert (to prevent spamming)
                                var timeSinceLastAlert = DateTime.UtcNow - (subscription.LastAlertSentAt ?? DateTime.MinValue);
                                if (timeSinceLastAlert.TotalMinutes < 15) // Only send alerts every 15 minutes
                                {
                                    continue;
                                }

                                // Send push notification if token exists
                                if (!string.IsNullOrEmpty(subscription.PushNotificationToken))
                                {
                                    var title = $"{area.AreaName} is getting crowded!";
                                    var message = $"Crowd level reached {areaOccupancyRate:F1}%. Consider visiting elsewhere.";
                                    var data = new Dictionary<string, string>
                                    {
                                        { "areaId", area.Id.ToString() },
                                        { "occupancyRate", areaOccupancyRate.ToString("F1") },
                                        { "densityLevel", densityLevel.ToString() },
                                        { "threshold", subscription.CrowdThresholdPercentage.ToString() }
                                    };

                                    var success = await pushNotificationService.SendPushNotificationAsync(
                                        subscription.PushNotificationToken,
                                        title,
                                        message,
                                        data
                                    );

                                    if (success)
                                    {
                                        subscription.LastAlertSentAt = DateTime.UtcNow;
                                        context.UserAreaSubscriptions.Update(subscription);
                                        _logger.LogInformation($"Alert sent to user {subscription.UserId} for area {area.AreaName}");
                                    }
                                }
                            }

                            // Save all updates
                            await context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error checking and sending alerts: {ex.Message}");
                }
            }
        }
    }
}
