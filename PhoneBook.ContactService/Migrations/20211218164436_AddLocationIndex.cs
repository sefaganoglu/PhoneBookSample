using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactService.Migrations
{
    public partial class AddLocationIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_Location",
                table: "ContactInfos",
                column: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactInfos_Location",
                table: "ContactInfos");
        }
    }
}
