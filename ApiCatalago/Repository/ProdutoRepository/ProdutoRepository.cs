using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Repository.RepositoryProduto.ProdutoRepository;
using System.Linq.Expressions;

namespace ApiCatalago.Repository.ProdutoRepository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApiCatalagoDbContext context) : base(context)
        {

        }



        public IEnumerable<Produto> GetProductByPrice()
        {
            throw new NotImplementedException();
        }
    }
}
