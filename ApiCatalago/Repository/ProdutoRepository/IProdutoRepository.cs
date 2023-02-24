using ApiCatalago.Models;

namespace ApiCatalago.Repository.RepositoryProduto.ProdutoRepository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProductByPrice();
    }
}
