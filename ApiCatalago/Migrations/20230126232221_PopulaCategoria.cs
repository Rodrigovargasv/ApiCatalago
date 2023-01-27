using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) VALUES('Bebidas', 'Cocacola.png')");
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) VALUES('Lanches', 'Lanches.png')");
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) VALUES('Sobremesas', 'Sobremesas.png')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
