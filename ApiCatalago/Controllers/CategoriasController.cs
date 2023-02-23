using ApiCatalago.Context;
using ApiCatalago.Models;
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

        public CategoriasController(ApiCatalagoDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {

            try
            {
                return await _context.Categorias.AsNoTracking().ToListAsync();
            }
            catch(Exception)
            {
                return  StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }

        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllAsync()
        {

            try
            {
                return await _context.Categorias.Include(p => p.Produtos).Where(c => c.Id <= 20).ToListAsync();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }


        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetIdAsync(int id)
        {
            try
            {
                var getId = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

                if (getId is null)
                {
                    return NotFound($"Categoria com id={id} não encontrada");
                }

                return Ok(getId);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.Id, categoria });
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }



        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> PutAsync(Categoria categoria, int id)
        {
            try
            {
                if (id != categoria.Id)
                {
                    return BadRequest($"id={id} não encontrado");
                }

                var p = _context.Categorias.Update(categoria);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpDelete("id:int")]
        public async Task<ActionResult> DeleteAsync(int id)
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

                return Ok();
            }
            catch(Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }


        }


    }
}


