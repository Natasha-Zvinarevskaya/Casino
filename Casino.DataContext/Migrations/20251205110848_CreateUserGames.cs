using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameHistory_PlayerGames_PlayerGameId",
                table: "GameHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSettings_PlayerGames_PlayerGameId",
                table: "GameSettings");

            migrationBuilder.DropTable(
                name: "PlayerGames");

            migrationBuilder.DropIndex(
                name: "IX_GameSettings_PlayerGameId",
                table: "GameSettings");

            migrationBuilder.RenameColumn(
                name: "PlayerGame",
                table: "UserTransactions",
                newName: "Game");

            migrationBuilder.RenameColumn(
                name: "PlayerGameId",
                table: "GameSettings",
                newName: "MaxCountPlayers");

            migrationBuilder.RenameColumn(
                name: "PlayerGameId",
                table: "GameHistory",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "CardsHistory",
                table: "GameHistory",
                newName: "History");

            migrationBuilder.RenameIndex(
                name: "IX_GameHistory_PlayerGameId",
                table: "GameHistory",
                newName: "IX_GameHistory_GameId");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "GameSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountBet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Games = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StripeCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethodId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardLast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StripeCustomers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersGames",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersGames", x => new { x.UserId, x.GameId });
                    table.ForeignKey(
                        name: "FK_UsersGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSettings_GameId",
                table: "GameSettings",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_UsersId",
                table: "Games",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactions_UserId",
                table: "MoneyTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StripeCustomers_UserId",
                table: "StripeCustomers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersGames_GameId",
                table: "UsersGames",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameHistory_Games_GameId",
                table: "GameHistory",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSettings_Games_GameId",
                table: "GameSettings",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameHistory_Games_GameId",
                table: "GameHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSettings_Games_GameId",
                table: "GameSettings");

            migrationBuilder.DropTable(
                name: "MoneyTransactions");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "StripeCustomers");

            migrationBuilder.DropTable(
                name: "UsersGames");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_GameSettings_GameId",
                table: "GameSettings");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "GameSettings");

            migrationBuilder.RenameColumn(
                name: "Game",
                table: "UserTransactions",
                newName: "PlayerGame");

            migrationBuilder.RenameColumn(
                name: "MaxCountPlayers",
                table: "GameSettings",
                newName: "PlayerGameId");

            migrationBuilder.RenameColumn(
                name: "History",
                table: "GameHistory",
                newName: "CardsHistory");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameHistory",
                newName: "PlayerGameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameHistory_GameId",
                table: "GameHistory",
                newName: "IX_GameHistory_PlayerGameId");

            migrationBuilder.CreateTable(
                name: "PlayerGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AmountBet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Game = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSettings_PlayerGameId",
                table: "GameSettings",
                column: "PlayerGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGames_UserId",
                table: "PlayerGames",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameHistory_PlayerGames_PlayerGameId",
                table: "GameHistory",
                column: "PlayerGameId",
                principalTable: "PlayerGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSettings_PlayerGames_PlayerGameId",
                table: "GameSettings",
                column: "PlayerGameId",
                principalTable: "PlayerGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
