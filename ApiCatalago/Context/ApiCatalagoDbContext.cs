using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Context
{
    // classe usada para fazer o mapeamento objeto relacional
    public class ApiCatalagoDbContext : DbContext
    {
        public ApiCatalagoDbContext(DbContextOptions<ApiCatalagoDbContext> options) : base(options) { }

        //MAPEAMENRTO
        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Produto>? Produtos { get; set; }
    }
}
