using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SubSystems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "RelativeBaseValue",
                table: "Characteristics",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "HealthScoreQuality",
                table: "Characteristics",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Characteristics",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "Characteristics",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HealthScoreQuality",
                table: "Assets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Assets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SubSystems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "HealthScoreQuality",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Assets");

            migrationBuilder.AlterColumn<decimal>(
                name: "RelativeBaseValue",
                table: "Characteristics",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "HealthScoreQuality",
                table: "Characteristics",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
