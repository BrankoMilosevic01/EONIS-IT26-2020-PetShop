using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShop.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DodataDetaljnaAdresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adresa",
                table: "AspNetUsers",
                newName: "Ulica");

            migrationBuilder.AddColumn<string>(
                name: "BrojStana",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KucniBroj",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mesto",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostanskiBroj",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojStana",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KucniBroj",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Mesto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostanskiBroj",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Ulica",
                table: "AspNetUsers",
                newName: "Adresa");
        }
    }
}
