using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdlens_backend.DTOs
{
    // Request DTOs
    public class SubscribeToAreaRequestDto
    {
        public int AreaId { get; set; }
        public double CrowdThresholdPercentage { get; set; } = 75.0;
        public string PushNotificationToken { get; set; } = string.Empty;
    }

    public class UpdateSubscriptionRequestDto
    {
        public double CrowdThresholdPercentage { get; set; }
        public bool IsNotificationEnabled { get; set; }
        public string? PushNotificationToken { get; set; }
    }

    public class SendPushNotificationRequestDto
    {
        public int AreaId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    // Response DTOs
    public class UserAreaSubscriptionDto
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public double CrowdThresholdPercentage { get; set; }
        public bool IsNotificationEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastAlertSentAt { get; set; }
        public double CurrentOccupancyRate { get; set; }
        public string CurrentDensityLevel { get; set; } = string.Empty;
    }

    public class AlertNotificationDto
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public double CurrentOccupancyRate { get; set; }
        public double ThresholdPercentage { get; set; }
        public DateTime SentAt { get; set; }
        public string DensityLevel { get; set; } = string.Empty;
    }

    public class SubscribedAreasResponseDto
    {
        public List<UserAreaSubscriptionDto> SubscribedAreas { get; set; } = new();
        public int TotalSubscriptions { get; set; }
    }
}
