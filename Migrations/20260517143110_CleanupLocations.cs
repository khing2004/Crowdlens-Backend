using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crowdlens_backend.Migrations
{
    /// <inheritdoc />
    public partial class CleanupLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix Vicente Sotto coordinates
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3080, \"Longitude\" = 123.8915 WHERE \"LocationName\" = 'Vicente Sotto Medical Center'");

            // Delete orphan ForecastRecords then the location rows
            migrationBuilder.Sql("DELETE FROM \"ForecastRecords\" WHERE \"LocationId\" IN (SELECT \"Id\" FROM \"Locations\" WHERE \"LocationName\" = 'Chong Hua Hospital')");
            migrationBuilder.Sql("DELETE FROM \"Locations\" WHERE \"LocationName\" = 'Chong Hua Hospital'");

            migrationBuilder.Sql("DELETE FROM \"ForecastRecords\" WHERE \"LocationId\" IN (SELECT \"Id\" FROM \"Locations\" WHERE \"LocationName\" = 'University of San Carlos - Main Campus')");
            migrationBuilder.Sql("DELETE FROM \"Locations\" WHERE \"LocationName\" = 'University of San Carlos - Main Campus'");

            migrationBuilder.Sql("DELETE FROM \"ForecastRecords\" WHERE \"LocationId\" IN (SELECT \"Id\" FROM \"Locations\" WHERE \"LocationName\" = 'Cebu Normal University')");
            migrationBuilder.Sql("DELETE FROM \"Locations\" WHERE \"LocationName\" = 'Cebu Normal University'");

            migrationBuilder.Sql("DELETE FROM \"ForecastRecords\" WHERE \"LocationId\" IN (SELECT \"Id\" FROM \"Locations\" WHERE \"LocationName\" = 'Colon Street')");
            migrationBuilder.Sql("DELETE FROM \"Locations\" WHERE \"LocationName\" = 'Colon Street'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
