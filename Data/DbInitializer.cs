using System;
using System.Collections.Generic;
using System.Linq;
using CrowdLens.Data;
using Crowdlens_backend.Models;
using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static void Seed(CrowdLensDbContext context)
    {
        context.Database.EnsureCreated();

        // --- Ensure ForecastRecords table exists for databases created before this feature ---
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""ForecastRecords"" (
                ""Id""           INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                ""LocationId""   INTEGER NOT NULL,
                ""RecordedAt""   TEXT    NOT NULL,
                ""DensityScore"" INTEGER NOT NULL,
                ""DensityLevel"" TEXT    NOT NULL
            )
        ");

        // --- Seed locations ---
        var locations = new List<Location>
        {
            // ── Existing ────────────────────────────────────────────────────────
            new Location
            {
                LocationName = "Cebu City Public Library",
                Type         = "Public Library",
                Capacity     = 300,
                UserCount    = 150,
                Latitude     = 10.3120,
                Longitude    = 123.8920,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 5, VotesMedium = 15, VotesHigh = 3, VotesVeryHigh = 0,
            },
            new Location
            {
                LocationName = "Vicente Sotto Medical Center",
                Type         = "Hospital",
                Capacity     = 1000,
                UserCount    = 850,
                Latitude     = 10.3080,
                Longitude    = 123.8915,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 0, VotesLow = 1, VotesMedium = 4, VotesHigh = 20, VotesVeryHigh = 25,
            },
            new Location
            {
                LocationName = "Fuente Osmeña Circle",
                Type         = "Public Square",
                Capacity     = 500,
                UserCount    = 50,
                Latitude     = 10.3098,
                Longitude    = 123.8931,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 10, VotesLow = 15, VotesMedium = 2, VotesHigh = 0, VotesVeryHigh = 0,
            },
            new Location
            {
                LocationName = "Eversley Childs Sanitarium and General Hospital",
                Type         = "Hospital",
                Capacity     = 1000,
                UserCount    = 850,
                Latitude     = 10.361734,
                Longitude    = 123.954351,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 13, VotesLow = 16, VotesMedium = 8, VotesHigh = 0, VotesVeryHigh = 0,
            },
            new Location
            {
                LocationName = "University of the Philippines Cebu Library",
                Type         = "Public Library",
                Capacity     = 300,
                UserCount    = 150,
                Latitude     = 10.3212,
                Longitude    = 123.8978,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 5, VotesLow = 10, VotesMedium = 15, VotesHigh = 20, VotesVeryHigh = 25,
            },

            // ── Shopping Malls ──────────────────────────────────────────────────
            new Location
            {
                LocationName = "SM City Cebu",
                Type         = "Shopping Mall",
                Capacity     = 15000,
                UserCount    = 8000,
                Latitude     = 10.3114,
                Longitude    = 123.9177,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 3, VotesMedium = 8, VotesHigh = 18, VotesVeryHigh = 20,
            },
            new Location
            {
                LocationName = "Ayala Center Cebu",
                Type         = "Shopping Mall",
                Capacity     = 12000,
                UserCount    = 6000,
                Latitude     = 10.3184,
                Longitude    = 123.9051,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 4, VotesMedium = 10, VotesHigh = 15, VotesVeryHigh = 18,
            },
            new Location
            {
                LocationName = "SM Seaside City Cebu",
                Type         = "Shopping Mall",
                Capacity     = 10000,
                UserCount    = 5500,
                Latitude     = 10.2823,
                Longitude    = 123.8812,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 4, VotesMedium = 9, VotesHigh = 14, VotesVeryHigh = 16,
            },
            new Location
            {
                LocationName = "Robinsons Galleria Cebu",
                Type         = "Shopping Mall",
                Capacity     = 8000,
                UserCount    = 4000,
                Latitude     = 10.3041,
                Longitude    = 123.9112,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 5, VotesMedium = 10, VotesHigh = 12, VotesVeryHigh = 8,
            },
            new Location
            {
                LocationName = "Gaisano Country Mall",
                Type         = "Shopping Mall",
                Capacity     = 5000,
                UserCount    = 2500,
                Latitude     = 10.3394,
                Longitude    = 123.9107,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 3, VotesLow = 8, VotesMedium = 12, VotesHigh = 9, VotesVeryHigh = 5,
            },
            new Location
            {
                LocationName = "Times Square Talamban",
                Type         = "Shopping Mall",
                Capacity     = 4000,
                UserCount    = 2000,
                Latitude     = 10.3694,
                Longitude    = 123.9182,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 3, VotesLow = 9, VotesMedium = 14, VotesHigh = 8, VotesVeryHigh = 4,
            },

            // ── Markets ─────────────────────────────────────────────────────────
            new Location
            {
                LocationName = "Carbon Market",
                Type         = "Market",
                Capacity     = 3000,
                UserCount    = 1800,
                Latitude     = 10.2994,
                Longitude    = 123.8990,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 3, VotesMedium = 6, VotesHigh = 20, VotesVeryHigh = 22,
            },
            new Location
            {
                LocationName = "Taboan Public Market",
                Type         = "Market",
                Capacity     = 1500,
                UserCount    = 900,
                Latitude     = 10.2955,
                Longitude    = 123.8910,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 5, VotesMedium = 8, VotesHigh = 16, VotesVeryHigh = 12,
            },

            // ── Churches ─────────────────────────────────────────────────────────
            new Location
            {
                LocationName = "Basilica Minore del Santo Niño",
                Type         = "Church",
                Capacity     = 2000,
                UserCount    = 800,
                Latitude     = 10.2941,
                Longitude    = 123.9020,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 5, VotesMedium = 10, VotesHigh = 18, VotesVeryHigh = 25,
            },
            new Location
            {
                LocationName = "Metropolitan Cathedral of Cebu",
                Type         = "Church",
                Capacity     = 1500,
                UserCount    = 500,
                Latitude     = 10.2955,
                Longitude    = 123.9029,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 3, VotesLow = 8, VotesMedium = 12, VotesHigh = 15, VotesVeryHigh = 10,
            },

            // ── Hospitals (additional) ───────────────────────────────────────────
            new Location
            {
                LocationName = "Cebu Doctors' University Hospital",
                Type         = "Hospital",
                Capacity     = 600,
                UserCount    = 450,
                Latitude     = 10.3144,
                Longitude    = 123.8919,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 3, VotesMedium = 7, VotesHigh = 15, VotesVeryHigh = 18,
            },

            // ── Government Office ────────────────────────────────────────────────
            new Location
            {
                LocationName = "Cebu City Hall",
                Type         = "Government Office",
                Capacity     = 500,
                UserCount    = 250,
                Latitude     = 10.2928,
                Longitude    = 123.9014,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 5, VotesMedium = 10, VotesHigh = 18, VotesVeryHigh = 5,
            },

            // ── Parks ───────────────────────────────────────────────────────────
            new Location
            {
                LocationName = "Plaza Independencia",
                Type         = "Park",
                Capacity     = 2000,
                UserCount    = 400,
                Latitude     = 10.2932,
                Longitude    = 123.9050,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 8, VotesLow = 12, VotesMedium = 8, VotesHigh = 5, VotesVeryHigh = 2,
            },

            // ── Transport Hubs ───────────────────────────────────────────────────
            new Location
            {
                LocationName = "Mactan-Cebu International Airport",
                Type         = "Transport Hub",
                Capacity     = 8000,
                UserCount    = 4000,
                Latitude     = 10.3073,
                Longitude    = 123.9796,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 5, VotesMedium = 10, VotesHigh = 20, VotesVeryHigh = 15,
            },
            new Location
            {
                LocationName = "Cebu South Bus Terminal",
                Type         = "Transport Hub",
                Capacity     = 2000,
                UserCount    = 1200,
                Latitude     = 10.2980,
                Longitude    = 123.8933,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 6, VotesMedium = 12, VotesHigh = 18, VotesVeryHigh = 10,
            },
            new Location
            {
                LocationName = "Pier 1 - Port of Cebu",
                Type         = "Transport Hub",
                Capacity     = 5000,
                UserCount    = 2500,
                Latitude     = 10.2929,
                Longitude    = 123.9078,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 8, VotesMedium = 14, VotesHigh = 16, VotesVeryHigh = 8,
            },

            // ── School Centers ──────────────────────────────────────────────────
            new Location
            {
                LocationName = "UP Cebu AS Conference Hall",
                Type         = "School Center",
                Capacity     = 8000,
                UserCount    = 4000,
                Latitude     = 10.3233,
                Longitude    = 123.8994,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 5, VotesLow = 10, VotesMedium = 18, VotesHigh = 12, VotesVeryHigh = 5,
            },

            // ── Food and Dining ─────────────────────────────────────────────────
            new Location
            {
                LocationName = "Kawayanan",
                Type         = "Food and Dining",
                Capacity     = 8000,
                UserCount    = 4000,
                Latitude     = 10.3237,
                Longitude    = 123.8992,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 5, VotesMedium = 10, VotesHigh = 18, VotesVeryHigh = 15,
            },
            new Location
            {
                LocationName = "McDonalds",
                Type         = "Food and Dining",
                Capacity     = 8000,
                UserCount    = 4000,
                Latitude     = 10.3306,
                Longitude    = 123.8982,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 2, VotesLow = 6, VotesMedium = 12, VotesHigh = 16, VotesVeryHigh = 14,
            },

            // ── Business Center ─────────────────────────────────────────────────
            new Location
            {
                LocationName = "Cebu IT Park",
                Type         = "Business Center",
                Capacity     = 3000,
                UserCount    = 2000,
                Latitude     = 10.3301,
                Longitude    = 123.9086,
                LastUpdated  = DateTime.Now,
                VotesVeryLow = 1, VotesLow = 3, VotesMedium = 6, VotesHigh = 20, VotesVeryHigh = 22,
            },
        };

        foreach (var location in locations)
        {
            if (!context.Locations.Any(l => l.LocationName == location.LocationName))
                context.Locations.Add(location);
        }
        context.SaveChanges();

        // Seed forecast records for each location that doesn't have any yet
        SeedForecastRecords(context);
    }

    // -----------------------------------------------------------------------
    //  Seeds 90 days of hourly forecast records only for locations that
    //  have no records yet, so existing data is never overwritten.
    // -----------------------------------------------------------------------
    private static void SeedForecastRecords(CrowdLensDbContext context)
    {
        var allLocations = context.Locations.ToList();
        var seededIds    = context.ForecastRecords
                                  .Select(f => f.LocationId)
                                  .Distinct()
                                  .ToHashSet();

        var toSeed = allLocations.Where(l => !seededIds.Contains(l.Id)).ToList();
        if (!toSeed.Any()) return;

        var records = new List<ForecastRecord>();
        var rng     = new Random(42);
        var now     = DateTime.Now;

        foreach (var location in toSeed)
        {
            for (int daysAgo = 90; daysAgo >= 1; daysAgo--)
            {
                var  date      = now.AddDays(-daysAgo).Date;
                bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday
                              || date.DayOfWeek == DayOfWeek.Sunday;

                int dayShift  = rng.NextDouble() < 0.25 ? (rng.Next(2) == 0 ? 1 : -1) : 0;
                int eventBump = rng.NextDouble() < 0.07 ? rng.Next(1, 3) : 0;

                for (int hour = 0; hour < 24; hour++)
                {
                    int baseScore = GetBaseScore(location.Type, location.LocationName, hour, isWeekend);

                    double u1      = 1.0 - rng.NextDouble();
                    double u2      = 1.0 - rng.NextDouble();
                    double gaussian = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
                    int    noise   = (int)Math.Round(gaussian * 0.8);

                    int score = Math.Clamp(baseScore + noise + dayShift + eventBump, 1, 5);

                    records.Add(new ForecastRecord
                    {
                        LocationId   = location.Id,
                        RecordedAt   = date.AddHours(hour),
                        DensityScore = score,
                        DensityLevel = ScoreToLevel(score)
                    });
                }
            }
        }

        context.ForecastRecords.AddRange(records);
        context.SaveChanges();
    }

    // -----------------------------------------------------------------------
    //  Routes each location to its busyness pattern.
    // -----------------------------------------------------------------------
    private static int GetBaseScore(
        string locationType, string locationName, int hour, bool isWeekend)
    {
        return locationType switch
        {
            "Hospital"         => GetHospitalScore(hour),
            "Public Library"   => locationName == "Cebu City Public Library"
                                      ? GetCCPLScore(hour)
                                      : GetUPLibraryScore(hour, isWeekend),
            "Public Square"    => GetPublicSquareScore(hour, isWeekend),
            "Park"             => GetParkScore(hour, isWeekend),
            "Shopping Mall"    => GetShoppingMallScore(hour, isWeekend),
            "Market"           => GetMarketScore(hour, isWeekend),
            "Church"           => GetChurchScore(hour, isWeekend),
            "University"       => GetUniversityScore(hour, isWeekend),
            "Government Office"=> GetGovernmentOfficeScore(hour, isWeekend),
            "Transport Hub"    => GetTransportHubScore(hour, isWeekend),
            "Food Strip"       => GetFoodStripScore(hour, isWeekend),
            "Food and Dining"  => GetFoodStripScore(hour, isWeekend),
            "School Center"    => GetSchoolCenterScore(hour, isWeekend),
            "Business Center"  => GetBusinessCenterScore(hour, isWeekend),
            _                  => GetPublicSquareScore(hour, isWeekend)
        };
    }

    // ── Existing patterns ───────────────────────────────────────────────────

    private static int GetHospitalScore(int hour) => hour switch
    {
        >= 0  and < 5  => 2,
        >= 5  and < 7  => 3,
        >= 7  and < 10 => 3,
        >= 10 and < 14 => 4,
        >= 14 and < 18 => 5,
        >= 18 and < 20 => 3,
        _              => 2
    };

    private static int GetUPLibraryScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 9  => 1,
            >= 9  and < 11 => 2,
            >= 11 and < 14 => 3,
            >= 14 and < 17 => 3,
            >= 17 and < 18 => 2,
            _              => 1
        };
        return hour switch
        {
            >= 0  and < 8  => 1,
            >= 8  and < 10 => 2,
            >= 10 and < 14 => 4,
            >= 14 and < 17 => 2,
            >= 17 and < 18 => 2,
            _              => 1
        };
    }

    private static int GetSchoolCenterScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 8  => 1,
            >= 8  and < 10 => 2,
            >= 10 and < 13 => 3,
            >= 13 and < 17 => 3,
            >= 17 and < 19 => 2,
            _              => 1
        };
        return hour switch
        {
            >= 0  and < 7  => 1,
            >= 7  and < 9  => 3,
            >= 9  and < 12 => 3, // peak morning sessions
            >= 12 and < 13 => 4, // lunch break
            >= 13 and < 17 => 2, // afternoon sessions
            >= 17 and < 19 => 2,
            _              => 1
        };
    }

    private static int GetCCPLScore(int hour) => hour switch
    {
        >= 0  and < 6  => 1,
        >= 6  and < 8  => 2,
        >= 8  and < 10 => 3,
        >= 10 and < 18 => 4,
        >= 18 and < 21 => 3,
        >= 21 and < 23 => 2,
        _              => 1
    };

    private static int GetPublicSquareScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 6  => 1,
            >= 6  and < 9  => 2,
            >= 9  and < 12 => 3,
            >= 12 and < 18 => 4,
            >= 18 and < 21 => 3,
            _              => 2
        };
        return hour switch
        {
            >= 0  and < 6  => 1,
            >= 6  and < 8  => 2,
            >= 8  and < 11 => 1,
            >= 11 and < 14 => 2,
            >= 14 and < 17 => 1,
            >= 17 and < 20 => 3,
            _              => 1
        };
    }

    // ── New patterns ────────────────────────────────────────────────────────

    // Parks: weekend mornings busy (joggers), midday peak; weekdays quiet
    private static int GetParkScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 5  => 1,
            >= 5  and < 8  => 3, // morning joggers
            >= 8  and < 12 => 4, // weekend peak
            >= 12 and < 16 => 3,
            >= 16 and < 20 => 4, // afternoon strollers
            >= 20 and < 22 => 2,
            _              => 1
        };
        return hour switch
        {
            >= 0  and < 5  => 1,
            >= 5  and < 7  => 2, // morning joggers
            >= 7  and < 17 => 1, // quiet on weekdays
            >= 17 and < 20 => 3, // after-work crowd
            _              => 1
        };
    }

    // Shopping Malls: open 10 AM–10 PM; packed on weekends, steady weekday afternoons
    private static int GetShoppingMallScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 10 => 1,
            >= 10 and < 12 => 3,
            >= 12 and < 20 => 5,
            >= 20 and < 22 => 3,
            _              => 1
        };
        return hour switch
        {
            >= 0  and < 10 => 1,
            >= 10 and < 12 => 2,
            >= 12 and < 14 => 3, // lunch crowd
            >= 14 and < 19 => 4,
            >= 19 and < 22 => 3,
            _              => 1
        };
    }

    // Markets: early-morning rush; wind down by afternoon; closed at night
    private static int GetMarketScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 4  => 1,
            >= 4  and < 7  => 4,
            >= 7  and < 10 => 5, // peak weekend morning
            >= 10 and < 13 => 4,
            >= 13 and < 16 => 2,
            _              => 1
        };
        return hour switch
        {
            >= 0  and < 4  => 1,
            >= 4  and < 8  => 4, // early-morning rush
            >= 8  and < 11 => 3,
            >= 11 and < 14 => 2,
            >= 14 and < 16 => 1,
            _              => 1
        };
    }

    // Churches: daily masses at 6 AM, noon, and 6 PM; packed on Sundays
    private static int GetChurchScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 5  => 1,
            >= 5  and < 7  => 3,
            >= 7  and < 12 => 5, // packed Sunday masses
            >= 12 and < 13 => 3, // noon mass
            >= 13 and < 17 => 2,
            >= 17 and < 20 => 3, // evening mass
            _              => 1
        };
        return hour switch
        {
            >= 0  and < 5  => 1,
            >= 5  and < 8  => 3, // morning masses
            >= 8  and < 12 => 2,
            >= 12 and < 13 => 3, // noon mass
            >= 13 and < 17 => 1,
            >= 17 and < 19 => 3, // evening mass
            _              => 1
        };
    }

    // Universities: busy during class hours Mon–Fri; largely empty on weekends
    private static int GetUniversityScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 8  and < 12 => 2, // some weekend classes
            _              => 1
        };
        return hour switch
        {
            >= 0  and < 7  => 1,
            >= 7  and < 8  => 3,
            >= 8  and < 12 => 5, // morning classes
            >= 12 and < 13 => 4, // lunch break
            >= 13 and < 17 => 5, // afternoon classes
            >= 17 and < 19 => 3, // evening classes / departures
            >= 19 and < 21 => 2,
            _              => 1
        };
    }

    // Government offices: strict 8 AM–5 PM weekdays; closed weekends
    private static int GetGovernmentOfficeScore(int hour, bool isWeekend)
    {
        if (isWeekend) return 1;
        return hour switch
        {
            >= 0  and < 8  => 1,
            >= 8  and < 9  => 3,
            >= 9  and < 12 => 4,
            >= 12 and < 13 => 2, // lunch
            >= 13 and < 16 => 4,
            >= 16 and < 17 => 3, // last transactions
            _              => 1
        };
    }

    // Transport hubs: constant flow; sharp morning and evening rush on weekdays
    private static int GetTransportHubScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 4  => 1,
            >= 4  and < 7  => 2,
            >= 7  and < 10 => 3,
            >= 10 and < 20 => 4,
            >= 20 and < 23 => 3,
            _              => 2
        };
        return hour switch
        {
            >= 0  and < 4  => 1,
            >= 4  and < 7  => 3,
            >= 7  and < 9  => 5, // morning rush
            >= 9  and < 12 => 3,
            >= 12 and < 14 => 3,
            >= 14 and < 17 => 3,
            >= 17 and < 20 => 5, // evening rush
            >= 20 and < 22 => 3,
            _              => 2
        };
    }

    // Food strips (IT Park, Colon): lunch rush + dinner / nightlife peak
    private static int GetFoodStripScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 9  => 1,
            >= 9  and < 11 => 2,
            >= 11 and < 15 => 4, // lunch
            >= 15 and < 17 => 3,
            >= 17 and < 23 => 5, // dinner + nightlife
            _              => 2
        };
        return hour switch
        {
            >= 0  and < 10 => 1,
            >= 10 and < 11 => 2,
            >= 11 and < 14 => 5, // lunch rush (office workers)
            >= 14 and < 17 => 2, // afternoon slump
            >= 17 and < 22 => 4, // dinner rush
            _              => 1
        };
    }

    // Business Center (IT Park): BPO hub — busy day shift AND night shift 24/7
    private static int GetBusinessCenterScore(int hour, bool isWeekend)
    {
        if (isWeekend) return hour switch
        {
            >= 0  and < 6  => 3, // night-shift BPO workers still active
            >= 6  and < 9  => 2,
            >= 9  and < 18 => 3, // weekend skeleton crew + some offices open
            >= 18 and < 22 => 3, // evening BPO shifts arriving
            _              => 4  // night shift peak
        };
        return hour switch
        {
            >= 0  and < 3  => 4, // US day-shift BPO workers (PH midnight)
            >= 3  and < 6  => 3, // winding down from night shift
            >= 6  and < 8  => 2, // early morning gap
            >= 8  and < 9  => 4, // day shift arriving
            >= 9  and < 12 => 5, // peak day shift
            >= 12 and < 13 => 4, // lunch break (staggered)
            >= 13 and < 18 => 5, // afternoon day shift peak
            >= 18 and < 20 => 3, // day shift out, mid-shift arriving
            _              => 4  // night shift fully in (20–23)
        };
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
