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
            var categorias = _context.Categorias.ToList();

            if (categorias is null)
            {
                return NotFound("Categoria não encontrada");
            }
            return Ok(categorias);

        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            var categoria = _context.Categorias.Include(p => p.Produtos).ToList();

            if (categoria is null)
            {
                return NotFound("Lista de Categoria e produtos não encontrado");
            }

            return Ok(categoria);
        }


        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetId(int id)
        {
            var getId = _context.Categorias.FirstOrDefault(p => p.Id == id);

            if (getId is null)
            {
                return NotFound("Categoria não encontrada");
            }

            return Ok(getId);
        }
        [HttpPost]
        public ActionResult Post(Categoria categoria)
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


        [HttpPut("{id:int}")]
        public ActionResult Put(Categoria categoria, int id)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            var p = _context.Categorias.Update(categoria);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("id:int")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.Id == id);

            if (categoria is null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Remove(categoria);
            _context.SaveChanges();

            return Ok();


        }


    }
}


