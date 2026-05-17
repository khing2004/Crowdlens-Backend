using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crowdlens_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocationCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3120, \"Longitude\" = 123.8920 WHERE \"LocationName\" = 'Cebu City Public Library'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3072, \"Longitude\" = 123.8890 WHERE \"LocationName\" = 'Vicente Sotto Medical Center'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3098, \"Longitude\" = 123.8931 WHERE \"LocationName\" = 'Fuente Osmeña Circle'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3212, \"Longitude\" = 123.8978 WHERE \"LocationName\" = 'University of the Philippines Cebu Library'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3114, \"Longitude\" = 123.9177 WHERE \"LocationName\" = 'SM City Cebu'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2823, \"Longitude\" = 123.8812 WHERE \"LocationName\" = 'SM Seaside City Cebu'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3041, \"Longitude\" = 123.9112 WHERE \"LocationName\" = 'Robinsons Galleria Cebu'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3394, \"Longitude\" = 123.9107 WHERE \"LocationName\" = 'Gaisano Country Mall'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2994, \"Longitude\" = 123.8990 WHERE \"LocationName\" = 'Carbon Market'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2955, \"Longitude\" = 123.8910 WHERE \"LocationName\" = 'Taboan Public Market'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2941, \"Longitude\" = 123.9020 WHERE \"LocationName\" = 'Basilica Minore del Santo Niño'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2955, \"Longitude\" = 123.9029 WHERE \"LocationName\" = 'Metropolitan Cathedral of Cebu'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3144, \"Longitude\" = 123.8919 WHERE \"LocationName\" = 'Cebu Doctors'' University Hospital'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2928, \"Longitude\" = 123.9014 WHERE \"LocationName\" = 'Cebu City Hall'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2932, \"Longitude\" = 123.9050 WHERE \"LocationName\" = 'Plaza Independencia'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2980, \"Longitude\" = 123.8933 WHERE \"LocationName\" = 'Cebu South Bus Terminal'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2929, \"Longitude\" = 123.9078 WHERE \"LocationName\" = 'Pier 1 - Port of Cebu'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.3298, \"Longitude\" = 123.9082 WHERE \"LocationName\" = 'Cebu IT Park'");
            migrationBuilder.Sql("UPDATE \"Locations\" SET \"Latitude\" = 10.2967, \"Longitude\" = 123.8989 WHERE \"LocationName\" = 'Colon Street'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
