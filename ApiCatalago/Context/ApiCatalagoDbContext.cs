using ApiCatalago.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Context
{
    // classe usada para fazer o mapeamento objeto relacional
    public class ApiCatalagoDbContext : IdentityDbContext
    {
       
        public ApiCatalagoDbContext(DbContextOptions<ApiCatalagoDbContext> options) : base(options) { }

        public ApiCatalagoDbContext()
        {

        }
        //Mapeamento das entidades
        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Produto>? Produtos { get; set; }
    }
}
