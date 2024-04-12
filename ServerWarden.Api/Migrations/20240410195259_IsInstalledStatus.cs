using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWarden.Api.Migrations
{
    /// <inheritdoc />
    public partial class IsInstalledStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInstalling",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInstalling",
                table: "Servers");
        }
    }
}
