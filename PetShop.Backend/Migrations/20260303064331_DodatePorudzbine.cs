using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DodatePorudzbine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Porudzbine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailKupca = table.Column<string>(type: "TEXT", nullable: true),
                    KupljeniProizvodi = table.Column<string>(type: "TEXT", nullable: true),
                    UkupnaCena = table.Column<decimal>(type: "TEXT", nullable: false),
                    DatumPorudzbine = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Porudzbine", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Porudzbine");
        }
    }
}
