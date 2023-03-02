using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.DTOS
{
    public class ProdutoDTO
    {
        public int Id { get; set; }

        public string? Nome { get; set; }
       
        public string? Descrição { get; set; }

        public decimal? Preco { get; set; }

        public string? ImageUrl { get; set; }

        public int? CategoriaId { get; set; }

    }
}
