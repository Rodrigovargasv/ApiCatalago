namespace ApiCatalago.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descrição { get; set; }
        public double? Preco { get; set; }
        public string? ImageUrl { get; set; }
        public float? Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public Categoria? Categoria { get; set; }
        public int? CategoriaId { get; set; }


    }
}
