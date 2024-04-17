using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerWarden.Api.Migrations
{
    /// <inheritdoc />
    public partial class ShouldBeRunning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsInstalling",
                table: "Servers",
                newName: "ShouldBeRunning");

            migrationBuilder.AddColumn<bool>(
                name: "ShouldBeInstalling",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldBeInstalling",
                table: "Servers");

            migrationBuilder.RenameColumn(
                name: "ShouldBeRunning",
                table: "Servers",
                newName: "IsInstalling");
        }
    }
}
