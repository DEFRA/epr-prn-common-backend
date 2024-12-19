﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedRecylingTargetsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RecyclingTargets",
                table: "RecyclingTargets");

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Year",
                keyValue: 2025);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Year",
                keyValue: 2026);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Year",
                keyValue: 2027);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Year",
                keyValue: 2028);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Year",
                keyValue: 2029);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Year",
                keyValue: 2030);

            migrationBuilder.DropColumn(
                name: "AluminiumTarget",
                table: "RecyclingTargets");

            migrationBuilder.DropColumn(
                name: "GlassRemeltTarget",
                table: "RecyclingTargets");

            migrationBuilder.DropColumn(
                name: "GlassTarget",
                table: "RecyclingTargets");

            migrationBuilder.DropColumn(
                name: "PaperTarget",
                table: "RecyclingTargets");

            migrationBuilder.DropColumn(
                name: "PlasticTarget",
                table: "RecyclingTargets");

            migrationBuilder.DropColumn(
                name: "SteelTarget",
                table: "RecyclingTargets");

            migrationBuilder.RenameColumn(
                name: "WoodTarget",
                table: "RecyclingTargets",
                newName: "Target");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "RecyclingTargets",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RecyclingTargets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "MaterialNameRT",
                table: "RecyclingTargets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecyclingTargets",
                table: "RecyclingTargets",
                column: "Id");

            migrationBuilder.InsertData(
                table: "RecyclingTargets",
                columns: new[] { "Id", "MaterialNameRT", "Target", "Year" },
                values: new object[,]
                {
                    { 1, "Paper", 0.75m, 2025 },
                    { 2, "Paper", 0.77m, 2026 },
                    { 3, "Paper", 0.79m, 2027 },
                    { 4, "Paper", 0.81m, 2028 },
                    { 5, "Paper", 0.83m, 2029 },
                    { 6, "Paper", 0.85m, 2030 },
                    { 7, "Glass", 0.74m, 2025 },
                    { 8, "Glass", 0.76m, 2026 },
                    { 9, "Glass", 0.78m, 2027 },
                    { 10, "Glass", 0.8m, 2028 },
                    { 11, "Glass", 0.82m, 2029 },
                    { 12, "Glass", 0.85m, 2030 },
                    { 13, "Aluminium", 0.61m, 2025 },
                    { 14, "Aluminium", 0.62m, 2026 },
                    { 15, "Aluminium", 0.63m, 2027 },
                    { 16, "Aluminium", 0.64m, 2028 },
                    { 17, "Aluminium", 0.65m, 2029 },
                    { 18, "Aluminium", 0.67m, 2030 },
                    { 19, "Steel", 0.8m, 2025 },
                    { 20, "Steel", 0.81m, 2026 },
                    { 21, "Steel", 0.82m, 2027 },
                    { 22, "Steel", 0.83m, 2028 },
                    { 23, "Steel", 0.84m, 2029 },
                    { 24, "Steel", 0.85m, 2030 },
                    { 25, "Plastic", 0.55m, 2025 },
                    { 26, "Plastic", 0.57m, 2026 },
                    { 27, "Plastic", 0.59m, 2027 },
                    { 28, "Plastic", 0.61m, 2028 },
                    { 29, "Plastic", 0.63m, 2029 },
                    { 30, "Plastic", 0.65m, 2030 },
                    { 31, "Wood", 0.45m, 2025 },
                    { 32, "Wood", 0.46m, 2026 },
                    { 33, "Wood", 0.47m, 2027 },
                    { 34, "Wood", 0.48m, 2028 },
                    { 35, "Wood", 0.49m, 2029 },
                    { 36, "Wood", 0.5m, 2030 },
                    { 37, "GlassRemelt", 0.75m, 2025 },
                    { 38, "GlassRemelt", 0.76m, 2026 },
                    { 39, "GlassRemelt", 0.77m, 2027 },
                    { 40, "GlassRemelt", 0.78m, 2028 },
                    { 41, "GlassRemelt", 0.79m, 2029 },
                    { 42, "GlassRemelt", 0.8m, 2030 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RecyclingTargets",
                table: "RecyclingTargets");

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "RecyclingTargets",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 42);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RecyclingTargets");

            migrationBuilder.DropColumn(
                name: "MaterialNameRT",
                table: "RecyclingTargets");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "RecyclingTargets",
                newName: "WoodTarget");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "RecyclingTargets",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "AluminiumTarget",
                table: "RecyclingTargets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GlassRemeltTarget",
                table: "RecyclingTargets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GlassTarget",
                table: "RecyclingTargets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaperTarget",
                table: "RecyclingTargets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PlasticTarget",
                table: "RecyclingTargets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SteelTarget",
                table: "RecyclingTargets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecyclingTargets",
                table: "RecyclingTargets",
                column: "Year");

            migrationBuilder.InsertData(
                table: "RecyclingTargets",
                columns: new[] { "Year", "AluminiumTarget", "GlassRemeltTarget", "GlassTarget", "PaperTarget", "PlasticTarget", "SteelTarget", "WoodTarget" },
                values: new object[,]
                {
                    { 2025, 0.61m, 0.75m, 0.74m, 0.75m, 0.55m, 0.8m, 0.45m },
                    { 2026, 0.62m, 0.76m, 0.76m, 0.77m, 0.57m, 0.81m, 0.46m },
                    { 2027, 0.63m, 0.77m, 0.78m, 0.79m, 0.59m, 0.82m, 0.47m },
                    { 2028, 0.64m, 0.78m, 0.8m, 0.81m, 0.61m, 0.83m, 0.48m },
                    { 2029, 0.65m, 0.79m, 0.82m, 0.83m, 0.63m, 0.84m, 0.49m },
                    { 2030, 0.67m, 0.8m, 0.85m, 0.85m, 0.65m, 0.85m, 0.5m }
                });
        }
    }
}
