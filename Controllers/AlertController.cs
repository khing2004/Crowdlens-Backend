using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLens.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crowdlens_backend.DTOs;
using Crowdlens_backend.Models;
using System.Security.Claims;
using Crowdlens_backend.Helpers;

namespace Crowdlens_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertController : ControllerBase
    {
        private readonly CrowdLensDbContext _context;

        public AlertController(CrowdLensDbContext context)
        {
            _context = context;
        }

        // Helper method to get current user ID
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        /// <summary>
        /// Subscribe user to an area with custom crowd threshold
        /// </summary>
        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToArea([FromBody] SubscribeToAreaRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not found");

                // Check if area exists
                var area = await _context.Areas.FindAsync(request.AreaId);
                if (area == null)
                    return NotFound($"Area with ID {request.AreaId} not found");

                // Check if user is already subscribed
                var existingSubscription = await _context.UserAreaSubscriptions
                    .FirstOrDefaultAsync(uas => uas.UserId == userId && uas.AreaId == request.AreaId);

                if (existingSubscription != null)
                    return BadRequest("User is already subscribed to this area");

                // Validate threshold percentage
                if (request.CrowdThresholdPercentage < 0 || request.CrowdThresholdPercentage > 100)
                    return BadRequest("Crowd threshold must be between 0 and 100");

                var subscription = new UserAreaSubscription
                {
                    UserId = userId,
                    AreaId = request.AreaId,
                    CrowdThresholdPercentage = request.CrowdThresholdPercentage,
                    PushNotificationToken = request.PushNotificationToken,
                    IsNotificationEnabled = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserAreaSubscriptions.Add(subscription);
                await _context.SaveChangesAsync();

                var responseDto = new UserAreaSubscriptionDto
                {
                    Id = subscription.Id,
                    AreaId = area.Id,
                    AreaName = area.AreaName,
                    CrowdThresholdPercentage = subscription.CrowdThresholdPercentage,
                    IsNotificationEnabled = subscription.IsNotificationEnabled,
                    CreatedAt = subscription.CreatedAt,
                    UpdatedAt = subscription.UpdatedAt,
                    LastAlertSentAt = subscription.LastAlertSentAt,
                    CurrentOccupancyRate = area.OccupancyRate,
                    CurrentDensityLevel = CrowdDensityHelper.GetLevel(area.OccupancyRate).ToString()
                };

                return Ok(new { message = "Successfully subscribed to area", data = responseDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all areas user is subscribed to
        /// </summary>
        [HttpGet("subscriptions")]
        public async Task<IActionResult> GetUserSubscriptions()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not found");

                var subscriptions = await _context.UserAreaSubscriptions
                    .Where(uas => uas.UserId == userId)
                    .Include(uas => uas.Area)
                    .OrderByDescending(uas => uas.CreatedAt)
                    .ToListAsync();

                var subscriptionDtos = subscriptions.Select(sub => new UserAreaSubscriptionDto
                {
                    Id = sub.Id,
                    AreaId = sub.Area.Id,
                    AreaName = sub.Area.AreaName,
                    CrowdThresholdPercentage = sub.CrowdThresholdPercentage,
                    IsNotificationEnabled = sub.IsNotificationEnabled,
                    CreatedAt = sub.CreatedAt,
                    UpdatedAt = sub.UpdatedAt,
                    LastAlertSentAt = sub.LastAlertSentAt,
                    CurrentOccupancyRate = sub.Area.OccupancyRate,
                    CurrentDensityLevel = CrowdDensityHelper.GetLevel(sub.Area.OccupancyRate).ToString()
                }).ToList();

                var response = new SubscribedAreasResponseDto
                {
                    SubscribedAreas = subscriptionDtos,
                    TotalSubscriptions = subscriptionDtos.Count
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get specific subscription details
        /// </summary>
        [HttpGet("subscription/{subscriptionId}")]
        public async Task<IActionResult> GetSubscription(int subscriptionId)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not found");

                var subscription = await _context.UserAreaSubscriptions
                    .Include(uas => uas.Area)
                    .FirstOrDefaultAsync(uas => uas.Id == subscriptionId && uas.UserId == userId);

                if (subscription == null)
                    return NotFound("Subscription not found");

                var responseDto = new UserAreaSubscriptionDto
                {
                    Id = subscription.Id,
                    AreaId = subscription.Area.Id,
                    AreaName = subscription.Area.AreaName,
                    CrowdThresholdPercentage = subscription.CrowdThresholdPercentage,
                    IsNotificationEnabled = subscription.IsNotificationEnabled,
                    CreatedAt = subscription.CreatedAt,
                    UpdatedAt = subscription.UpdatedAt,
                    LastAlertSentAt = subscription.LastAlertSentAt,
                    CurrentOccupancyRate = subscription.Area.OccupancyRate,
                    CurrentDensityLevel = CrowdDensityHelper.GetLevel(subscription.Area.OccupancyRate).ToString()
                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update subscription settings (threshold, notification token, notification enabled)
        /// </summary>
        [HttpPut("subscription/{subscriptionId}")]
        public async Task<IActionResult> UpdateSubscription(int subscriptionId, [FromBody] UpdateSubscriptionRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not found");

                var subscription = await _context.UserAreaSubscriptions
                    .FirstOrDefaultAsync(uas => uas.Id == subscriptionId && uas.UserId == userId);

                if (subscription == null)
                    return NotFound("Subscription not found");

                // Validate threshold percentage
                if (request.CrowdThresholdPercentage < 0 || request.CrowdThresholdPercentage > 100)
                    return BadRequest("Crowd threshold must be between 0 and 100");

                subscription.CrowdThresholdPercentage = request.CrowdThresholdPercentage;
                subscription.IsNotificationEnabled = request.IsNotificationEnabled;
                if (!string.IsNullOrEmpty(request.PushNotificationToken))
                    subscription.PushNotificationToken = request.PushNotificationToken;
                subscription.UpdatedAt = DateTime.UtcNow;

                _context.UserAreaSubscriptions.Update(subscription);
                await _context.SaveChangesAsync();

                var area = await _context.Areas.FindAsync(subscription.AreaId);
                var responseDto = new UserAreaSubscriptionDto
                {
                    Id = subscription.Id,
                    AreaId = subscription.AreaId,
                    AreaName = area?.AreaName ?? string.Empty,
                    CrowdThresholdPercentage = subscription.CrowdThresholdPercentage,
                    IsNotificationEnabled = subscription.IsNotificationEnabled,
                    CreatedAt = subscription.CreatedAt,
                    UpdatedAt = subscription.UpdatedAt,
                    LastAlertSentAt = subscription.LastAlertSentAt,
                    CurrentOccupancyRate = area?.OccupancyRate ?? 0,
                    CurrentDensityLevel = area != null ? CrowdDensityHelper.GetLevel(area.OccupancyRate).ToString() : string.Empty
                };

                return Ok(new { message = "Subscription updated successfully", data = responseDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Unsubscribe user from an area
        /// </summary>
        [HttpDelete("unsubscribe/{subscriptionId}")]
        public async Task<IActionResult> UnsubscribeFromArea(int subscriptionId)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not found");

                var subscription = await _context.UserAreaSubscriptions
                    .FirstOrDefaultAsync(uas => uas.Id == subscriptionId && uas.UserId == userId);

                if (subscription == null)
                    return NotFound("Subscription not found");

                _context.UserAreaSubscriptions.Remove(subscription);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Successfully unsubscribed from area" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if a specific area breaches user's threshold
        /// Returns notification if threshold is exceeded
        /// </summary>
        [HttpGet("check-alert/{areaId}")]
        public async Task<IActionResult> CheckAreaAlert(int areaId)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not found");

                var subscription = await _context.UserAreaSubscriptions
                    .FirstOrDefaultAsync(uas => uas.UserId == userId && uas.AreaId == areaId && uas.IsNotificationEnabled);

                if (subscription == null)
                    return NotFound("You are not subscribed to this area or notifications are disabled");

                var area = await _context.Areas.FindAsync(areaId);
                if (area == null)
                    return NotFound("Area not found");

                // Check if current occupancy exceeds user's threshold
                if (area.OccupancyRate >= subscription.CrowdThresholdPercentage)
                {
                    var alert = new AlertNotificationDto
                    {
                        Id = subscription.Id,
                        AreaId = area.Id,
                        AreaName = area.AreaName,
                        Title = $"{area.AreaName} is getting crowded!",
                        Message = $"Crowd level has reached {area.OccupancyRate:F1}%, exceeding your threshold of {subscription.CrowdThresholdPercentage}%",
                        CurrentOccupancyRate = area.OccupancyRate,
                        ThresholdPercentage = subscription.CrowdThresholdPercentage,
                        SentAt = DateTime.UtcNow,
                        DensityLevel = CrowdDensityHelper.GetLevel(area.OccupancyRate).ToString()
                    };

                    // Update last alert sent timestamp
                    subscription.LastAlertSentAt = DateTime.UtcNow;
                    _context.UserAreaSubscriptions.Update(subscription);
                    await _context.SaveChangesAsync();

                    return Ok(new { shouldNotify = true, alert });
                }

                return Ok(new { shouldNotify = false, message = "Area occupancy is within acceptable levels" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all alerts for areas user is subscribed to (areas exceeding thresholds)
        /// </summary>
        [HttpGet("active-alerts")]
        public async Task<IActionResult> GetActiveAlerts()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not found");

                var subscriptions = await _context.UserAreaSubscriptions
                    .Where(uas => uas.UserId == userId && uas.IsNotificationEnabled)
                    .Include(uas => uas.Area)
                    .ToListAsync();

                var activeAlerts = subscriptions
                    .Where(sub => sub.Area.OccupancyRate >= sub.CrowdThresholdPercentage)
                    .Select(sub => new AlertNotificationDto
                    {
                        Id = sub.Id,
                        AreaId = sub.Area.Id,
                        AreaName = sub.Area.AreaName,
                        Title = $"{sub.Area.AreaName} is getting crowded!",
                        Message = $"Crowd level is {sub.Area.OccupancyRate:F1}%, exceeding your threshold of {sub.CrowdThresholdPercentage}%",
                        CurrentOccupancyRate = sub.Area.OccupancyRate,
                        ThresholdPercentage = sub.CrowdThresholdPercentage,
                        SentAt = DateTime.UtcNow,
                        DensityLevel = CrowdDensityHelper.GetLevel(sub.Area.OccupancyRate).ToString()
                    })
                    .ToList();

                return Ok(new { activeAlerts, totalAlerts = activeAlerts.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
