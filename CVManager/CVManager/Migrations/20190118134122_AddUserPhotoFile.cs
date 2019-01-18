using Microsoft.EntityFrameworkCore.Migrations;

namespace CVManager.Migrations
{
    public partial class AddUserPhotoFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName",
                table: "JobApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoFileName",
                table: "JobApplications");
        }
    }
}
