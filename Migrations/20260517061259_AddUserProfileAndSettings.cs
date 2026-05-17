using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crowdlens_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileAndSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarBase64",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "LocationSharingEnabled",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationsEnabled",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Pronouns",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarBase64",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LocationSharingEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NotificationsEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Pronouns",
                table: "AspNetUsers");
        }
    }
}
