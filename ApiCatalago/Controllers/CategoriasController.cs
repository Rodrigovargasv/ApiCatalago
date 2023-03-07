using ApiCatalago.Context;
using ApiCatalago.DTOS;
using ApiCatalago.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly ApiCatalagoDbContext _context;

        // definado um instancia de  IMapper
        private readonly IMapper _mapper; 

        // injentando serviços via construtor
        public CategoriasController(ApiCatalagoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
        {

            try
            {
                var categorias =  await _context.Categorias.AsNoTracking().ToListAsync();

                // mapeando categorias para um lista de CategoriasDTO
                var CategoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return Ok(CategoriasDto);
            }
            catch(Exception)
            {
                return  StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }

        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllAsync()
        {

            try
            {
                var categorias = await _context.Categorias.Include(p => p.Produtos).Where(c => c.Id <= 20).ToListAsync();

                // mapeando categorias para um lista de CategoriasDTO
                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return Ok(categoriaDto);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }


        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetIdAsync(int id)
        {
            try
            {
                var getId = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

                if (getId is null)
                {
                    return NotFound($"Categoria com id={id} não encontrada");
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(getId);

                return Ok(categoriaDto);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(CategoriaDTO categoriaDto)
        {
            try
            {
              
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                if (categoria is null)
                {
                    return BadRequest();
                }

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.Id, categoriaDTO });
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }



        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> PutAsync(CategoriaDTO categoriaDto, int id)
        {
            try
            {
                if (id != categoriaDto.Id)
                {
                    return BadRequest($"id={id} não encontrado");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.Categorias.Update(categoria);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpDelete("id:int")]
        public async Task<ActionResult<CategoriaDTO>> DeleteAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                {
                    return NotFound("Categoria com id={id} não encontrado");
                }

                _context.Remove(categoria);
                await _context.SaveChangesAsync();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDto);
            }
            catch(Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }


        }


    }
}


