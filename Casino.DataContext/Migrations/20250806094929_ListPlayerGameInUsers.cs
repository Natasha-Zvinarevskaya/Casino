using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ListPlayerGameInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PlayerGames_PlayerGameId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PlayerGameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PlayerGameId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PlayerGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGames_UserId",
                table: "PlayerGames",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerGames_Users_UserId",
                table: "PlayerGames",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerGames_Users_UserId",
                table: "PlayerGames");

            migrationBuilder.DropIndex(
                name: "IX_PlayerGames_UserId",
                table: "PlayerGames");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlayerGames");

            migrationBuilder.AddColumn<int>(
                name: "PlayerGameId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PlayerGameId",
                table: "Users",
                column: "PlayerGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PlayerGames_PlayerGameId",
                table: "Users",
                column: "PlayerGameId",
                principalTable: "PlayerGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
