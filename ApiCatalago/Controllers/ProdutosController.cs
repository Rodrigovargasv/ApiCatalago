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

            try
            {
                return _context.Produtos.AsNoTracking().ToList();
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        // traz o produtos com id especificado no parametro
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> GetId(int id)
        {
            try
            {
                var getID = _context.Produtos.AsNoTracking().FirstOrDefault(x => x.Id == id);

                if (getID is null)
                {
                    return NotFound($"Produto com id={id} não encontrado");
                }
                return Ok(getID);
            }
            catch(Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        //cria um produto na tabela produtos
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                if (produto is null)
                {
                    return BadRequest("Não foi posssivel criar um novo produto");
                }

                _context.Produtos.Add(produto);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id, produto });
            }
            catch(Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
;        }


        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(Produto produto, int id) 
        {
            try
            {
                if (id != produto.Id)
                {
                    return BadRequest($"NÃO foi possivel atualiza o produto pois o Id={id} não existe");
                }

                var p = _context.Produtos.Update(produto);
                _context.SaveChanges();

                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpDelete("id:int:min(1)")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

                if (produto is null)
                {
                    return NotFound($"Produto com id={id} não encontrado");
                }

                _context.Remove(produto);
                _context.SaveChanges();

                return Ok();

            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }
    }
}
