using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalago.Models
{
    //não obrigatorio
    [Table("Categorias")]
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string? Nome { get; set; }

        [Required]
        [MaxLength(300)]  
        public string? ImageUrl { get; set; }
        public ICollection<Produto>? Produtos { get; set; }

        public Categoria()
        {
            Produtos = new List<Produto>();
        }
    }
}
