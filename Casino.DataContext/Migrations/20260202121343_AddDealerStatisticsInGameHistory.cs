using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddDealerStatisticsInGameHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DealerCheating",
                table: "GameHistory",
                newName: "WinCheating");

            migrationBuilder.AddColumn<bool>(
                name: "Cheating",
                table: "GameHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCrook",
                table: "GameHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Risk",
                table: "GameHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cheating",
                table: "GameHistory");

            migrationBuilder.DropColumn(
                name: "IsCrook",
                table: "GameHistory");

            migrationBuilder.DropColumn(
                name: "Risk",
                table: "GameHistory");

            migrationBuilder.RenameColumn(
                name: "WinCheating",
                table: "GameHistory",
                newName: "DealerCheating");
        }
    }
}
