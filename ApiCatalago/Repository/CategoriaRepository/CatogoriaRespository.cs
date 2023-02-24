using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Repository.CategoriaRepository
{
    public class CatogoriaRespository : Repository<Categoria>, ICategoriaRepository
    {
        public CatogoriaRespository(ApiCatalagoDbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetCategoryAndProduct()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
