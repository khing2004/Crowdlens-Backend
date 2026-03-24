namespace Crowdlens_backend.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Type { get; set; } = "Public Space"; // Added to match frontend 'type'
        
        // Voting Data
        public int VotesVeryLow { get; set; }
        public int VotesLow { get; set; }
        public int VotesMedium { get; set; }
        public int VotesHigh { get; set; }
        public int VotesVeryHigh { get; set; }

        public int UserCount { get; set; }
        public int Capacity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double OccupancyRate => Capacity > 0 ? (double)UserCount / Capacity * 100 : 0;
    }
}