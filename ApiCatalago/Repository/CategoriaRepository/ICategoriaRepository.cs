
using ApiCatalago.Models;


namespace ApiCatalago.Repository.CategoriaRepository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoryAndProduct();
       
    }
}
