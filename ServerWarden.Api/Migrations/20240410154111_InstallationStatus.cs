using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWarden.Api.Migrations
{
    /// <inheritdoc />
    public partial class InstallationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBeenInstalled",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenInstalled",
                table: "Servers");
        }
    }
}
