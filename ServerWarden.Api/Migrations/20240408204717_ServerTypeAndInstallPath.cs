using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWarden.Api.Migrations
{
    /// <inheritdoc />
    public partial class ServerTypeAndInstallPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstallationPath",
                table: "Servers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ServerType",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallationPath",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "ServerType",
                table: "Servers");
        }
    }
}
