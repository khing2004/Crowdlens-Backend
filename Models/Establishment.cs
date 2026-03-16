using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdlens_backend.Models
{
    public class Establishment
    {

        public int Id { get; set; }

        public string EstablishmentName { get; set; } = string.Empty;

        public int UserCount { get; set; }
        
        public DateTime updatedOn { get; set; } = DateTime.UtcNow;

        public int? AreaId { get; set; }

        public Area? Area { get; set; }

        public double Longitude { get; set; }

        public int Latitude { get; set; }
    }
}