using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdlens_backend.Helpers
{
    public enum DensityLevel
    {
        VeryLow,
        Low,
        Moderate,
        High,
        VeryHigh

    }

    public static class CrowdDensityHelper
    {
        public static DensityLevel GetLevel(double OccupancyRate) => OccupancyRate switch
        {
            < 20  => DensityLevel.VeryLow,
            < 40  => DensityLevel.Low,
            < 60  => DensityLevel.Moderate,
            < 80  => DensityLevel.High,
            _     => DensityLevel.VeryHigh
        };

        public static string GetColor(DensityLevel level) => level switch
        {
            DensityLevel.VeryLow  => "#4CAF50",  // green
            DensityLevel.Low      => "#8BC34A",  // light green
            DensityLevel.Moderate => "#FFC107",  // amber
            DensityLevel.High     => "#FF5722",  // orange
            DensityLevel.VeryHigh => "#F44336",  // red
            _                     => "#9E9E9E"   // grey fallback
        };

        public static string GetTimestampLabel(DateTime lastUpdated)
        {
            var diff = DateTime.UtcNow - lastUpdated;
            return diff.TotalMinutes < 1    ? "updated just now"
                 : diff.TotalMinutes < 60   ? $"updated {(int)diff.TotalMinutes} minutes ago"
                 : diff.TotalHours < 24     ? $"updated {(int)diff.TotalHours} hours ago"
                 : $"updated {(int)diff.TotalDays} days ago";
        }

        public static bool IsFeedStale(DateTime lastUpdated, int thresholdMinutes = 15)
            => (DateTime.UtcNow - lastUpdated).TotalMinutes > thresholdMinutes;
    }
}