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
        public ActionResult<IEnumerable<Categoria>> Get()
        {

            try
            {
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch(Exception)
            {
                return  StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }

        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {

            try
            {
                return _context.Categorias.Include(p => p.Produtos).Where(c => c.Id <= 20).ToList();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }


        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetId(int id)
        {
            try
            {
                var getId = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.Id == id);

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
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }

                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.Id, categoria });
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }



        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(Categoria categoria, int id)
        {
            try
            {
                if (id != categoria.Id)
                {
                    return BadRequest($"id={id} não encontrado");
                }

                var p = _context.Categorias.Update(categoria);
                _context.SaveChanges();

                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpDelete("id:int")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(x => x.Id == id);

                if (categoria is null)
                {
                    return NotFound("Categoria com id={id} não encontrado");
                }

                _context.Remove(categoria);
                _context.SaveChanges();

                return Ok();
            }
            catch(Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }


        }


    }
}


