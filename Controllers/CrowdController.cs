using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLens.Data;
using Crowdlens_backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crowdlens_backend.DTOs;
using Crowdlens_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Crowdlens_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrowdController : ControllerBase
    {
        private readonly CrowdLensDbContext _context; // what does this do? My initial idea of this line is using CrowdlensDb as the context or the reference but its done in private and cannot be modified

        public CrowdController(CrowdLensDbContext context)
        {
            _context = context;
        }
        // what does line 16 to 19 do? My initial idea is to initialize crowdcontroller as the main context for the database so, instead of calling crowdlensdbcontext.context everytime, we simply call crowdcontroller?

        [HttpGet("area/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAreaCrowdLevel(int id)
        {
            try
            {
                var area = await _context.Areas
                    .Include(a => a.Establishments)
                    .Include(a => a.AlternativeAreas)
                    .FirstOrDefaultAsync(a => a.Id == id);

                // What does line 30 to line 33 do? Initial thoughts: 
                if(area == null)
                    return NotFound($"Area with ID {id} not found.");

                var isFeedStale = CrowdDensityHelper.IsFeedStale(area.LastUpdated);
                var densityLevel = CrowdDensityHelper.GetLevel(area.OccupancyRate);

                var response = new AreaCrowdResponseDto
                {
                    Id = area.Id,
                    AreaName = area.AreaName,
                    UserCount = area.UserCount,
                    Capacity = area.Capacity,
                    OccupancyRate = Math.Round(area.OccupancyRate, 1),
                    DensityLevel = densityLevel.ToString(),
                    DensityColor = CrowdDensityHelper.GetColor(densityLevel),
                    LastUpdated = CrowdDensityHelper.GetTimestampLabel(area.LastUpdated),
                    IsLiveFeed = !isFeedStale,
                    IsFeedStale = isFeedStale,
                    Latitude = area.Latitude,
                    Longitude = area.Longitude,

                    Establishments = area.Establishments.Select(e =>
                    {
                        var eDensity = CrowdDensityHelper.GetLevel(e.OccupancyRate);
                        return new EstablishmentCrowdDto
                        {
                            Id = e.Id,
                            EstablishmentName = e.EstablishmentName,
                            UserCount = e.UserCount,
                            Capacity = e.Capacity,
                            OccupancyRate = Math.Round(e.OccupancyRate, 1),
                            DensityLevel = eDensity.ToString(),
                            DensityColor = CrowdDensityHelper.GetColor(eDensity),
                            LastUpdated = CrowdDensityHelper.GetTimestampLabel(e.UpdatedOn),
                            Latitude = e.Latitude,
                            Longitude = e.Longitude
                        };
                    }).ToList(),

                    // Only show alternative if area is High or Very High
                    AlternativeAreas = densityLevel >= DensityLevel.High
                        ? area.AlternativeAreas.Select(alt =>
                        {
                            var altDensity = CrowdDensityHelper.GetLevel(alt.OccupancyRate);
                            return new AlternativeAreaDto
                            {
                                Id = alt.Id,
                                AreaName = alt.AreaName,
                                OccupancyRate = Math.Round(alt.OccupancyRate, 1),
                                DensityLevel = altDensity.ToString(),
                                DensityColor = CrowdDensityHelper.GetColor(altDensity),
                                Latitude = alt.Latitude,
                                Longitude = alt.Longitude
                            };
                        }).ToList()
                        : new()
                };   // What does area.AlternativeAreas.Select(alt => {}) mean? is the inside of the bracket a function?

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unable to retrieve crowd data.",
                    error = ex.Message
                });
            }
        }


    }
}