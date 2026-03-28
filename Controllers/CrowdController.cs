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

        // get all locations with basic info for map display
        [HttpGet("locations")]
        [Authorize]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);

            var dtos = new List<CrowdLocationsDto>();

            foreach (var l in locations)
            {
                // 1. Get fresh votes for THIS location
                var freshVotes = await _context.Reports
                    .Where(r => r.LocationId == l.Id && r.CreatedAt >= oneHourAgo)
                    .ToListAsync();

                string finalDensity;

                if (freshVotes.Any())
                {
                    // 2. Logic: Find the level with the MOST votes (The Mode)
                    finalDensity = freshVotes
                        .GroupBy(v => v.SelectedLevel)
                        .OrderByDescending(g => g.Count())
                        .First()
                        .Key ?? "Very Low";
                    
                }
                else
                {
                    // 3. Fallback: If no one has voted recently, use the sensor/occupancy data
                    finalDensity = CrowdDensityHelper.GetLevel(l.OccupancyRate);
                }

                dtos.Add(new CrowdLocationsDto
                {
                    id = l.Id,
                    name = l.LocationName, // Ensure this maps to 'name' for the frontend
                    type = l.Type,
                    pos = new List<double> { l.Latitude, l.Longitude },
                    density = finalDensity, // This string now matches your crowdHelper keys
                    lastUpdated = CrowdDensityHelper.GetTimestampLabel(l.LastUpdated)
                });

            }

            return Ok(dtos);
}

        [HttpGet("location/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLocationCrowdLevel(int id)
        {
            try
            {
                // 1. Fetch the location data from the database
                var location = await _context.Locations
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (location == null)
                    return NotFound($"Location with ID {id} not found.");

                // 2. Aggregate Fresh Votes (Last 60 minutes) for real-time accuracy
                var oneHourAgo = DateTime.UtcNow.AddHours(-1);
                var freshVotes = await _context.Reports
                    .Where(r => r.LocationId == id && r.CreatedAt >= oneHourAgo)
                    .ToListAsync();

                // 3. Map to the DTO structure required by the React frontend
                // Note: 'Pos' is set as [Latitude, Longitude] to match your React interface
                var response = new CrowdLocationsDto
                {
                    id = location.Id,
                    name = location.LocationName,
                    type = location.Type,
                    pos = new List<double> { location.Latitude, location.Longitude },
                    userCount = location.UserCount,
                    capacity = location.Capacity,
                    occupancyRate = Math.Round(location.OccupancyRate, 1),
                    density = CrowdDensityHelper.GetLevel(location.OccupancyRate).ToString(),
                    lastUpdated = CrowdDensityHelper.GetTimestampLabel(location.LastUpdated),

                    // You could also add a Votes property here if you update your DTO
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server Error", error = ex.Message });
            }
        }

        [HttpPost("report")]
        [Authorize]
        public async Task<IActionResult> SubmitReport([FromBody] Report reportRequest)
        {
            if (reportRequest == null) return BadRequest("Invalid report data.");


            var userId = User.Identity?.Name; // using JWT to get user ID in Name
            var cooldownPeriod = DateTime.UtcNow.AddMinutes(-15); // 15-minute cooldown
            
            // Check if the user has already reported for this location within the cooldown period
            bool hasRecentVote = await _context.Reports
                .AnyAsync(r => r.LocationId == reportRequest.LocationId 
                            && r.UserId == userId 
                            && r.CreatedAt >= cooldownPeriod);

            if (hasRecentVote)
            {
                return BadRequest("You have already reported for this location within the last 15 minutes.");
            }

            // Create a new report record
            var newReport = new Report
            {
                LocationId = reportRequest.LocationId,
                SelectedLevel = reportRequest.SelectedLevel,
                UserId = User.Identity?.Name, // Track which user is voting via JWT
                CreatedAt = DateTime.UtcNow
            };

            _context.Reports.Add(newReport);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Crowd level reported successfully." });
        }
    }
}