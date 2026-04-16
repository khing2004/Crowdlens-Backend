using CrowdLens.Data;
using Crowdlens_backend.DTOs;
using Crowdlens_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crowdlens_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastController : ControllerBase
    {
        private readonly CrowdLensDbContext _context;

        public ForecastController(CrowdLensDbContext context)
        {
            _context = context;
        }

        // GET /api/Forecast/{locationId}?hoursAhead=6
        [HttpGet("{locationId}")]
        [Authorize]
        public async Task<IActionResult> GetForecast(int locationId, [FromQuery] int hoursAhead = 6)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null)
                    return NotFound(new { message = $"Location {locationId} not found." });

                // Load all historical records for this location once
                var allRecords = await _context.ForecastRecords
                    .Where(r => r.LocationId == locationId)
                    .ToListAsync();

                var now       = DateTime.Now;
                var forecast  = new List<ForecastSlotDto>();

                for (int i = 0; i < hoursAhead; i++)
                {
                    var target  = now.AddHours(i);
                    var slotHour = target.Hour;
                    var slotDow  = target.DayOfWeek;

                    // Match records with the same hour-of-day AND same day-of-week
                    var matching = allRecords
                        .Where(r => r.RecordedAt.Hour == slotHour
                                 && r.RecordedAt.DayOfWeek == slotDow)
                        .ToList();

                    forecast.Add(ComputeSlot(target, matching, now));
                }

                // Suggest an alternative only when the peak predicted score is High or Very High
                ForecastAlternativeDto? alternative = null;
                int peakScore = forecast.Max(f => f.DensityScore);
                if (peakScore >= 4)
                    alternative = await FindBestAlternative(locationId, now, hoursAhead);

                return Ok(new ForecastResponseDto
                {
                    LocationId          = locationId,
                    LocationName        = location.LocationName,
                    Forecast            = forecast,
                    SuggestedAlternative = alternative,
                    ForecastUnavailable = false
                });
            }
            catch
            {
                // Return a graceful fallback so the frontend can show its error state
                return Ok(new ForecastResponseDto { ForecastUnavailable = true });
            }
        }

        // -----------------------------------------------------------------------
        //  FORECASTING ALGORITHM — Weighted Temporal Pattern Average
        //
        //  For each future hour slot:
        //    1. Retrieve all historical records matching (same hour-of-day, same day-of-week).
        //    2. Apply exponential decay weighting: w = exp(−daysAgo / 14)
        //       → records from two weeks ago contribute half as much as today's.
        //    3. Compute weighted mean → round to nearest integer score (1–5).
        //    4. Compute variance → derive confidence percentage.
        //    5. Flag LowDataWarning when fewer than 3 samples exist.
        // -----------------------------------------------------------------------
        private static ForecastSlotDto ComputeSlot(
            DateTime target,
            List<ForecastRecord> matching,
            DateTime now)
        {
            if (!matching.Any())
            {
                return new ForecastSlotDto
                {
                    Hour           = target.ToString("HH:mm"),
                    IsoTime        = target.ToString("o"),
                    DensityScore   = 2,
                    DensityLevel   = "Low",
                    ConfidencePct  = 20,
                    LowDataWarning = true
                };
            }

            // Exponential decay weight (half-life = 14 days)
            double totalWeight  = 0;
            double weightedSum  = 0;

            foreach (var r in matching)
            {
                double daysAgo = (now - r.RecordedAt).TotalDays;
                double weight  = Math.Exp(-daysAgo / 14.0);
                weightedSum   += r.DensityScore * weight;
                totalWeight   += weight;
            }

            double weightedMean = weightedSum / totalWeight;

            // Standard deviation (unweighted — reflects spread across historical data)
            double variance = matching.Average(r => Math.Pow(r.DensityScore - weightedMean, 2));
            double stdDev   = Math.Sqrt(variance);

            // Confidence: rises with sample count, falls with high variance
            int basePct      = Math.Min(95, 45 + matching.Count * 5);
            int stdDevPenalty = (int)(stdDev * 8);
            int confidencePct = Math.Max(20, basePct - stdDevPenalty);

            int score = (int)Math.Round(Math.Clamp(weightedMean, 1, 5));

            return new ForecastSlotDto
            {
                Hour           = target.ToString("HH:mm"),
                IsoTime        = target.ToString("o"),
                DensityScore   = score,
                DensityLevel   = ScoreToLevel(score),
                ConfidencePct  = confidencePct,
                LowDataWarning = matching.Count < 3
            };
        }

        /// <summary>
        /// Finds the location (other than the requested one) with the lowest peak
        /// forecasted density over the same window.  Only returned when that peak
        /// is ≤ 3 (Medium or below) — i.e. a meaningfully better alternative exists.
        /// </summary>
        private async Task<ForecastAlternativeDto?> FindBestAlternative(
            int excludeLocationId,
            DateTime now,
            int hoursAhead)
        {
            var otherLocations = await _context.Locations
                .Where(l => l.Id != excludeLocationId)
                .ToListAsync();

            ForecastAlternativeDto? best     = null;
            int                     bestPeak = int.MaxValue;

            foreach (var loc in otherLocations)
            {
                var records = await _context.ForecastRecords
                    .Where(r => r.LocationId == loc.Id)
                    .ToListAsync();

                int peakScore = 1;
                for (int i = 0; i < hoursAhead; i++)
                {
                    var target   = now.AddHours(i);
                    var matching = records
                        .Where(r => r.RecordedAt.Hour      == target.Hour
                                 && r.RecordedAt.DayOfWeek == target.DayOfWeek)
                        .ToList();

                    int slotScore = ComputeSlot(target, matching, now).DensityScore;
                    if (slotScore > peakScore) peakScore = slotScore;
                }

                if (peakScore < bestPeak)
                {
                    bestPeak = peakScore;
                    best     = new ForecastAlternativeDto
                    {
                        LocationId       = loc.Id,
                        LocationName     = loc.LocationName,
                        PeakDensityScore = peakScore,
                        PeakDensityLevel = ScoreToLevel(peakScore)
                    };
                }
            }

            // Only suggest when the alternative is meaningfully less congested
            return (best != null && bestPeak <= 3) ? best : null;
        }

        private static string ScoreToLevel(int score) => score switch
        {
            1 => "Very Low",
            2 => "Low",
            3 => "Medium",
            4 => "High",
            5 => "Very High",
            _ => "Unknown"
        };
    }
}
