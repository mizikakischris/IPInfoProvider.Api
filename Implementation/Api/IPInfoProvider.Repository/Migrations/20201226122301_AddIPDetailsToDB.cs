using Microsoft.EntityFrameworkCore.Migrations;

namespace IPInfoProvider.Repository.Migrations
{
    public partial class AddIPDetailsToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IPDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(nullable: true),
                    Country_Name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Continent_Name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IPDetails");
        }
    }
}
