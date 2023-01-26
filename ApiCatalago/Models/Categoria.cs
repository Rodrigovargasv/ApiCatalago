using Microsoft.VisualBasic;

namespace ApiCatalago.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Produto>? Produtos { get; set; }

        public Categoria()
        {
            Produtos = new List<Produto>();
        }
    }
}
