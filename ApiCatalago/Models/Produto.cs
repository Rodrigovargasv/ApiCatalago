﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalago.Models
{
    //não obrigatorio
    [Table("Produtos")]
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? Descrição { get; set; }

        [Column(TypeName ="decimal(8,2)")]
        [Required]
        public decimal? Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }

        public float? Estoque { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        public int? CategoriaId { get; set; }


    }
}
