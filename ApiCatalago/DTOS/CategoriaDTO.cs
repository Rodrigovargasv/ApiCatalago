using ApiCatalago.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.DTOS
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
   
        public string? Nome { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<ProdutoDTO>? Produtos { get; set; }
    }
}
