using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibrary.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaQuantidadeLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeDisponivel",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeTotal",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeDisponivel",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "QuantidadeTotal",
                table: "Livros");
        }
    }
}
