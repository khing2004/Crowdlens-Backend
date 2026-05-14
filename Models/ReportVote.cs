namespace Crowdlens_backend.Models
{
    public class ReportVote
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string UserId { get; set; } = "";
        public string VoteType { get; set; } = ""; // "Up" or "Down"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Report Report { get; set; } = null!;
    }
}
