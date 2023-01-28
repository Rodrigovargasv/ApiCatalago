using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // instancia a conexão com banco de dados
        private readonly ApiCatalagoDbContext _context;

        // injeta a dependencia
        public ProdutosController(ApiCatalagoDbContext context)
        {
            _context = context;
        }

        // traz todos os produtos na tabela produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return produtos;
        }

        // traz o produtos com id especificado no parametro
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> GetId(int id)
        {
            var getID = _context.Produtos.FirstOrDefault(x => x.Id == id);

            if (getID is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return Ok(getID);
        }

        //cria um produto na tabela produtos
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest();
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new {id = produto.Id, produto });
;        }


        [HttpPut("{id:int}")]
        public ActionResult Put(Produto produto, int id) 
        {
            if (id != produto.Id) 
            { 
                return BadRequest();
            }

            var p = _context.Produtos.Update(produto);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("id:int")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x =>x.Id == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Remove(produto);
            _context.SaveChanges();

            return Ok();

        }
    }
}
