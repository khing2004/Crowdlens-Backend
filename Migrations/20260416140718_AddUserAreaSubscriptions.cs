using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crowdlens_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAreaSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAreaSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false),
                    CrowdThresholdPercentage = table.Column<double>(type: "REAL", nullable: false),
                    IsNotificationEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PushNotificationToken = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastAlertSentAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAreaSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAreaSubscriptions_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAreaSubscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAreaSubscriptions_AreaId",
                table: "UserAreaSubscriptions",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAreaSubscriptions_UserId_AreaId",
                table: "UserAreaSubscriptions",
                columns: new[] { "UserId", "AreaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAreaSubscriptions");
        }
    }
}
