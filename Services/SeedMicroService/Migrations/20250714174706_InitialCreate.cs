using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SeedMicroService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Species = table.Column<string>(type: "varchar(200)", nullable: false),
                    RisingTime = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationBeforeHarvest = table.Column<int>(type: "INTEGER", nullable: false),
                    Picture = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seeds_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Légume feuille" },
                    { 2, "Légume fruit" },
                    { 3, "Légume racine" }
                });

            migrationBuilder.InsertData(
                table: "Seeds",
                columns: new[] { "Id", "CategoryId", "DurationBeforeHarvest", "Name", "Picture", "RisingTime", "Species" },
                values: new object[,]
                {
                    { new Guid("17e8c645-6149-44c2-8702-73867d6fef7a"), 2, 120, "Géante de New York", "", 10, "Aubergine" },
                    { new Guid("a8edc8f5-55b3-4ac7-9c23-7f9e4ca696ee"), 1, 30, "Batavia", "", 5, "Laitue" },
                    { new Guid("f1996cad-fddf-44f3-bc72-990708096c1b"), 2, 90, "Saint Pierre", "", 3, "Tomate" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seeds_CategoryId",
                table: "Seeds",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seeds");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
