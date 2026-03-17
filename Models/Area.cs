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
        public List<Establishment> Establishments { get; set; } = new(); // 
        public List<Area> AlternativeAreas { get; set; } = new(); // suggested locations when user limit is reached

        public int UserCount { get; set; }
        public int Capacity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        //Computed and not stored in DB
        public double OccupancyRate => Capacity > 0 ? (double)UserCount / Capacity * 100 : 0;
    }
}