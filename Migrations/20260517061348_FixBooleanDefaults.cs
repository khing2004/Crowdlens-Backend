using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crowdlens_backend.Migrations
{
    /// <inheritdoc />
    public partial class FixBooleanDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Set existing users' new boolean columns to true (1)
            migrationBuilder.Sql(
                "UPDATE \"AspNetUsers\" SET \"NotificationsEnabled\" = 1, \"LocationSharingEnabled\" = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
