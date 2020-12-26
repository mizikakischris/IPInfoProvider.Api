using Microsoft.EntityFrameworkCore.Migrations;

namespace IPInfoProvider.Repository.Migrations
{
    public partial class AddIPToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IP_Adress",
                table: "IPDetails",
                type: "nvarchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IP_Adress",
                table: "IPDetails");
        }
    }
}
