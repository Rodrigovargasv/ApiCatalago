using ApiCatalago.Models;
using AutoMapper;

namespace ApiCatalago.DTOS.Mappings
{
    public class MappingProfile : Profile
    {
        // mapeamento dos models para DTOS
        public MappingProfile()
        { 
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
