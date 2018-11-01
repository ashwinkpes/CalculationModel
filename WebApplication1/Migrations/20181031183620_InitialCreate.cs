using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    HealthScore = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    AssetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                });

            migrationBuilder.CreateTable(
                name: "SubSystems",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    HealthScore = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    SubSystemId = table.Column<Guid>(nullable: false),
                    AssetId = table.Column<Guid>(nullable: false),
                    SubSystemWeight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSystems", x => x.SubSystemId);
                    table.ForeignKey(
                        name: "FK_SubSystems_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    HealthScore = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    CharacteristicId = table.Column<Guid>(nullable: false),
                    UsedInCalculation = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    CalculationType = table.Column<int>(nullable: false),
                    RelativeBaseValue = table.Column<decimal>(nullable: false),
                    Min = table.Column<double>(nullable: false),
                    Max = table.Column<double>(nullable: false),
                    Value1 = table.Column<double>(nullable: false),
                    Value2 = table.Column<double>(nullable: false),
                    Value3 = table.Column<double>(nullable: false),
                    Value4 = table.Column<double>(nullable: false),
                    DataAgeScale = table.Column<int>(nullable: false),
                    DataAgeMax = table.Column<int>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    HealthScoreQuality = table.Column<int>(nullable: false),
                    SubSystemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.CharacteristicId);
                    table.ForeignKey(
                        name: "FK_Characteristics_SubSystems_SubSystemId",
                        column: x => x.SubSystemId,
                        principalTable: "SubSystems",
                        principalColumn: "SubSystemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_SubSystemId",
                table: "Characteristics",
                column: "SubSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SubSystems_AssetId",
                table: "SubSystems",
                column: "AssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DropTable(
                name: "SubSystems");

            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
