using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class EnumGamesInPlayerGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Game",
                table: "PlayerGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Game",
                table: "PlayerGames");
        }
    }
}
