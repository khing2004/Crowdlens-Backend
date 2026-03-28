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
        private readonly CrowdLensDbContext _context;

        public CrowdController(CrowdLensDbContext context)
        {
            _context = context;
        }

        //  Helper: Normalize density labels
        private string NormalizeLevel(string? level)
        {
            if (string.IsNullOrEmpty(level)) return "";

            return level switch
            {
                "Moderate" => "Medium",
                _ => level
            };
        }

        //  Helper: Get density from votes OR fallback
        private string ComputeDensity(List<string> votes)
        {
            if (votes.Any())
            {
                return votes
                    .GroupBy(v => v)
                    .OrderByDescending(g => g.Count())
                    .First()
                    .Key;
            }

            return "Very Low"; // fallback
        }

        //  GET ALL LOCATIONS
        [HttpGet("locations")]
        [Authorize]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            var oneHourAgo = DateTime.Now.AddHours(-1);

            //  Fetch ALL recent reports in ONE query
            var reports = await _context.Reports
                .Where(r => r.CreatedAt >= oneHourAgo)
                .ToListAsync();

            var dtos = new List<CrowdLocationsDto>();
            
            foreach (var l in locations)
            {
                // Filter reports per location + normalize + remove nulls
                var votes = reports
                    .Where(r => r.LocationId == l.Id && !string.IsNullOrEmpty(r.SelectedLevel))
                    .Select(r => NormalizeLevel(r.SelectedLevel))
                    .ToList();

                var finalDensity = ComputeDensity(votes);

                var voteCounts = votes
                        .GroupBy(v => v)
                        .ToDictionary(g => g.Key, g => g.Count());

                dtos.Add(new CrowdLocationsDto
                {
                    id = l.Id,
                    name = l.LocationName,
                    type = l.Type,
                    pos = new List<double> { l.Latitude, l.Longitude },
                    density = finalDensity,
                    lastUpdated = CrowdDensityHelper.GetTimestampLabel(l.LastUpdated),
                    votes = new Dictionary<string, int>
                    {
                        { "Very Low", voteCounts.GetValueOrDefault("Very Low", 0) },
                        { "Low", voteCounts.GetValueOrDefault("Low", 0) },
                        { "Medium", voteCounts.GetValueOrDefault("Medium", 0) },
                        { "High", voteCounts.GetValueOrDefault("High", 0) },
                        { "Very High", voteCounts.GetValueOrDefault("Very High", 0) }
                    }
                });
            }

            return Ok(dtos);
        }

        //  GET SINGLE LOCATION
        [HttpGet("location/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLocationCrowdLevel(int id)
        {
            try
            {
                var location = await _context.Locations
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (location == null)
                    return NotFound($"Location with ID {id} not found.");

                var oneHourAgo = DateTime.Now.AddHours(-1);

                var votes = await _context.Reports
                    .Where(r => r.LocationId == id
                             && r.CreatedAt >= oneHourAgo
                             && !string.IsNullOrEmpty(r.SelectedLevel))
                    .Select(r => NormalizeLevel(r.SelectedLevel))
                    .ToListAsync();

                var finalDensity = ComputeDensity(votes);

                var response = new CrowdLocationsDto
                {
                    id = location.Id,
                    name = location.LocationName,
                    type = location.Type,
                    pos = new List<double> { location.Latitude, location.Longitude },

                    userCount = location.UserCount,
                    capacity = location.Capacity,
                    occupancyRate = Math.Round(location.OccupancyRate, 1),

                    //  NOW CONSISTENT with map
                    density = finalDensity,

                    lastUpdated = CrowdDensityHelper.GetTimestampLabel(location.LastUpdated)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Server Error",
                    error = ex.Message
                });
            }
        }

        //  SUBMIT REPORT
        [HttpPost("report")]
        [Authorize]
        public async Task<IActionResult> SubmitReport([FromBody] Report reportRequest)
        {
            if (reportRequest == null)
                return BadRequest("Invalid report data.");

            var userId = User.Identity?.Name;

            //  Prevent null user bug
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid user identity.");

            var cooldownPeriod = DateTime.Now.AddMinutes(-15);

            bool hasRecentVote = await _context.Reports
                .AnyAsync(r =>
                    r.LocationId == reportRequest.LocationId &&
                    r.UserId == userId &&
                    r.CreatedAt >= cooldownPeriod);

            if (hasRecentVote)
            {
                return BadRequest("You have already reported for this location within the last 15 minutes.");
            }

            var newReport = new Report
            {
                LocationId = reportRequest.LocationId,
                SelectedLevel = NormalizeLevel(reportRequest.SelectedLevel),
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            _context.Reports.Add(newReport);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Crowd level reported successfully." });
        }
    }
}