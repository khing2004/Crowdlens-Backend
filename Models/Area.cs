using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdlens_backend.Models
{
    public class Area
    {
        public int Id { get; set; }
        
        public string AreaName { get; set; } = string.Empty;

        public List<Establishment> Establishments { get; set; } = new List<Establishment>();

        public int UserCount { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}