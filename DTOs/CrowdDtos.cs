using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdlens_backend.DTOs
{
    public class CrowdDtos
    {
        public class EstablishmentCrowdDto
    {
        public int Id { get; set; }
        public string EstablishmentName { get; set; } = "";
        public int UserCount { get; set; }
        public int Capacity { get; set; }
        public double OccupancyRate { get; set; }
        public string DensityLevel { get; set; } = "";
        public string DensityColor { get; set; } = "";
        public string LastUpdated { get; set; } = "";   // "updated 2 minutes ago"
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class AlternativeAreaDto
    {
        public int Id { get; set; }
        public string AreaName { get; set; } = "";
        public double OccupancyRate { get; set; }
        public string DensityLevel { get; set; } = "";
        public string DensityColor { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class AreaCrowdResponseDto
    {
        public int Id { get; set; }
        public string AreaName { get; set; } = "";
        public int UserCount { get; set; }
        public int Capacity { get; set; }
        public double OccupancyRate { get; set; }
        public string DensityLevel { get; set; } = "";
        public string DensityColor { get; set; } = "";
        public string LastUpdated { get; set; } = "";
        public bool IsLiveFeed { get; set; }            // false = fallback data
        public bool IsFeedStale { get; set; }           // true = connection may be lost
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<EstablishmentCrowdDto> Establishments { get; set; } = new();
        public List<AlternativeAreaDto> AlternativeAreas { get; set; } = new();
    }
    }
}