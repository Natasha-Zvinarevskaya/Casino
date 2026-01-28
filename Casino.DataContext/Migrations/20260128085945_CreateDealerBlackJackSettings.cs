using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class CreateDealerBlackJackSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerSettingas");


            migrationBuilder.CreateTable(
                name: "DealerBlackJackSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountPoints = table.Column<int>(type: "int", nullable: false),
                    PercentRisk = table.Column<float>(type: "real", nullable: false),
                    PercentСheating = table.Column<float>(type: "real", nullable: false),
                    PercentWinCheating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerBlackJackSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerBlackJackSettings");

            migrationBuilder.CreateTable(
                name: "DealerSettingas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountPoints = table.Column<int>(type: "int", nullable: false),
                    PercentRisk = table.Column<float>(type: "real", nullable: false),
                    PercentWinCheating = table.Column<float>(type: "real", nullable: false),
                    PercentСheating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerSettingas", x => x.Id);
                });
        }
    }
}
