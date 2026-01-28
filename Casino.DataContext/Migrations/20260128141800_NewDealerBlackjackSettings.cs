using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class NewDealerBlackjackSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_DealerBlackJackSettings",
            //    table: "DealerBlackJackSettings");

            //migrationBuilder.DropColumn(
            //    name: "CountPoints",
            //    table: "DealerBlackJackSettings");

            //migrationBuilder.DropColumn(
            //    name: "PercentRisk",
            //    table: "DealerBlackJackSettings");

            //migrationBuilder.DropColumn(
            //    name: "PercentWinCheating",
            //    table: "DealerBlackJackSettings");

            //migrationBuilder.DropColumn(
            //    name: "PercentСheating",
            //    table: "DealerBlackJackSettings");

            //migrationBuilder.RenameTable(
            //    name: "DealerBlackJackSettings",
            //    newName: "DealerBlackjackSettings");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "DealerBlackjackSettings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<string>(
            //    name: "Settings",
            //    table: "DealerBlackjackSettings",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_DealerBlackjackSettings",
            //    table: "DealerBlackjackSettings",
            //    column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_DealerBlackjackSettings",
            //    table: "DealerBlackjackSettings");

            //migrationBuilder.DropColumn(
            //    name: "Settings",
            //    table: "DealerBlackjackSettings");

            //migrationBuilder.RenameTable(
            //    name: "DealerBlackjackSettings",
            //    newName: "DealerBlackJackSettings");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "DealerBlackJackSettings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<int>(
            //    name: "CountPoints",
            //    table: "DealerBlackJackSettings",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<float>(
            //    name: "PercentRisk",
            //    table: "DealerBlackJackSettings",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<float>(
            //    name: "PercentWinCheating",
            //    table: "DealerBlackJackSettings",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<float>(
            //    name: "PercentСheating",
            //    table: "DealerBlackJackSettings",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_DealerBlackJackSettings",
            //    table: "DealerBlackJackSettings",
            //    column: "Id");
        }
    }
}
