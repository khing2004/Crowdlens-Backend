using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLens.Data;
using Crowdlens_backend.Models;

public static class DbInitializer
{
    public static void Seed(CrowdLensDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Areas.Any()) return; // Prevent duplicate seeding
            // 1. Create Areas
        var uptown = new Area
        {
            AreaName = "Uptown District",
            Capacity = 1500,
            UserCount = 1300, // Very High (86%)
            Latitude = 10.3200,
            Longitude = 123.8900,
            LastUpdated = DateTime.UtcNow
        };

        var downtown = new Area
        {
            AreaName = "Downtown Hub",
            Capacity = 2000,
            UserCount = 400, // Low (20%)
            Latitude = 10.3000,
            Longitude = 123.8800,
            LastUpdated = DateTime.UtcNow
        };

        var parkSide = new Area
        {
            AreaName = "Parkside Square",
            Capacity = 800,
            UserCount = 400, // Moderate (50%)
            Latitude = 10.3100,
            Longitude = 123.9000,
            LastUpdated = DateTime.UtcNow
        };

        // 2. Link Alternative Areas (For when Uptown is full)
        uptown.AlternativeAreas.Add(downtown);
        uptown.AlternativeAreas.Add(parkSide);

        // 3. Add Establishments per Area
        var establishments = new List<Establishment>
        {
            // Uptown Establishments
            new Establishment { EstablishmentName = "Skyline Gym", Capacity = 100, UserCount = 95, Area = uptown, Latitude = 10.3201, Longitude = 123.8901 },
            new Establishment { EstablishmentName = "Uptown Cafe", Capacity = 40, UserCount = 38, Area = uptown, Latitude = 10.3205, Longitude = 123.8905 },

            // Downtown Establishments
            new Establishment { EstablishmentName = "Main Library", Capacity = 300, UserCount = 50, Area = downtown, Latitude = 10.3001, Longitude = 123.8801 },
            new Establishment { EstablishmentName = "Old Town Bistro", Capacity = 60, UserCount = 12, Area = downtown, Latitude = 10.3005, Longitude = 123.8805 },

            // Parkside Establishments
            new Establishment { EstablishmentName = "Greenery Co-working", Capacity = 50, UserCount = 25, Area = parkSide, Latitude = 10.3101, Longitude = 123.9001 }
        };

        context.Areas.AddRange(uptown, downtown, parkSide);
        context.Establishments.AddRange(establishments);
        
        context.SaveChanges();
    }
}