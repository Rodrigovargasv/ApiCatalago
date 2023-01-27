using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO "+
                    "Produtos (Nome, Descrição, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                    "VALUES ('Cocacola', 'Bebida em lata', 5.50, 'CocaCola.jpge', 10, GETDATE(), 1)");
            mb.Sql("INSERT INTO " +
                    "Produtos (Nome, Descrição, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                    "VALUES ('Xtudo', 'Lanche completo', 15.00, 'Xtudo.jpge', 2, GETDATE(), 2)");
            mb.Sql("INSERT INTO " +
                    "Produtos (Nome, Descrição, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                    "VALUES ('Danix', 'Pacaote de biscoito recheado', 2.5, 'Daniz.jpge', 25, GETDATE(), 3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
