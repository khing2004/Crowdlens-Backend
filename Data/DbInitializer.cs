using System;
using System.Collections.Generic;
using System.Linq;
using CrowdLens.Data;
using Crowdlens_backend.Models;

public static class DbInitializer
{
    public static void Seed(CrowdLensDbContext context)
    {
        // Ensures the database exists before seeding
        context.Database.EnsureCreated();

        // Check if we have already seeded locations to prevent duplicates
        if (context.Locations.Any()) return; 

        // Create Mock Locations based on your Cebu City coordinates
        var locations = new List<Location>
        {
            new Location
            {
                LocationName = "Cebu City Public Library",
                Type = "Public Library",
                Capacity = 300,
                UserCount = 150, // 50% Occupancy (Medium)
                Latitude = 10.3095,
                Longitude = 123.8931,
                LastUpdated = DateTime.UtcNow,
                // Initialize votes
                VotesVeryLow = 2,
                VotesLow = 5,
                VotesMedium = 15,
                VotesHigh = 3,
                VotesVeryHigh = 0
            },
            new Location
            {
                LocationName = "Vicente Sotto Medical Center",
                Type = "Hospital",
                Capacity = 1000,
                UserCount = 850, // 85% Occupancy (High)
                Latitude = 10.3117,
                Longitude = 123.8915,
                LastUpdated = DateTime.UtcNow,
                VotesVeryLow = 0,
                VotesLow = 1,
                VotesMedium = 4,
                VotesHigh = 20,
                VotesVeryHigh = 25
            },
            new Location
            {
                LocationName = "Fuente Osmeña Circle",
                Type = "Public Square",
                Capacity = 500,
                UserCount = 50, // 10% Occupancy (Low)
                Latitude = 10.3111,
                Longitude = 123.8941,
                LastUpdated = DateTime.UtcNow,
                VotesVeryLow = 10,
                VotesLow = 15,
                VotesMedium = 2,
                VotesHigh = 0,
                VotesVeryHigh = 0
            }
        };

        // Add the locations to the context
        context.Locations.AddRange(locations);
        
        // Seed some initial Reports (votes) to test "Fresh Votes" logic
        var initialReports = new List<Report>
        {
            new Report { LocationId = 1, SelectedLevel = "Medium", CreatedAt = DateTime.UtcNow.AddMinutes(-5) },
            new Report { LocationId = 2, SelectedLevel = "High", CreatedAt = DateTime.UtcNow.AddMinutes(-2) }
        };
        
        context.Reports.AddRange(initialReports);

        // Save all changes to crowdlens.db
        context.SaveChanges();
    }
}