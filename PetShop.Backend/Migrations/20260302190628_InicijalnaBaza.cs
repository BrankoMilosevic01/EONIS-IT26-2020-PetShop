using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InicijalnaBaza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proizvodi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false),
                    Cena = table.Column<decimal>(type: "TEXT", nullable: false),
                    KolicinaNaStanju = table.Column<int>(type: "INTEGER", nullable: false),
                    Slika = table.Column<string>(type: "TEXT", nullable: false),
                    KategorijaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Potkategorija = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodi", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proizvodi");
        }
    }
}
