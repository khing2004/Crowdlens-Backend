using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdlens_backend.Models
{
    public class UserAreaSubscription
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public string UserId { get; set; } = string.Empty; // Links to AspNetUsers table
        public int AreaId { get; set; }
        
        // Navigation Properties
        public User User { get; set; } = null!;
        public Area Area { get; set; } = null!;
        
        // Custom threshold for crowd alert (percentage)
        // e.g., 75 means alert user when occupancy reaches 75%
        public double CrowdThresholdPercentage { get; set; } = 75.0;
        
        // Whether notifications are enabled for this subscription
        public bool IsNotificationEnabled { get; set; } = true;
        
        // Push notification token for mobile device
        public string PushNotificationToken { get; set; } = string.Empty;
        
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Track last alert sent to avoid spamming
        public DateTime? LastAlertSentAt { get; set; }
    }
}
