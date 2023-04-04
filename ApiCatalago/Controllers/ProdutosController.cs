using ApiCatalago.Context;
using ApiCatalago.DTOS;
using ApiCatalago.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProdutosController : ControllerBase
    {
        // instancia a conexão com banco de dados
        private readonly ApiCatalagoDbContext _context;

        // injetando serviço via construtor
        private readonly IMapper _mapper;

        // injeta a dependencia via construtor
        public ProdutosController(ApiCatalagoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }  

        // traz todos os produtos na tabela produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync()
        {

            try
            {

                var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

                var produtoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

                return Ok(produtoDTO);
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        // traz o produtos com id especificado no parametro
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetIdAsync(int id)
        {
            try
            {
                var getID = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (getID is null)
                {
                    return NotFound($"Produto com id={id} não encontrado");
                }

                var produtosDto = _mapper.Map<Produto>(getID);
                return Ok(produtosDto);
            }
            catch(Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        //cria um produto na tabela produtos
        [HttpPost]
        public async Task<ActionResult> PostAsync(ProdutoDTO produtoDto)
        {
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);

                if (produto is null)
                {
                    return BadRequest("Não foi posssivel criar um novo produto");
                }

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id, produtoDTO});
            }
            catch(Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
;        }


        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> PutAsync(ProdutoDTO produtoDto, int id) 
        {
            try
            {
                if (id != produtoDto.Id)
                {
                    return BadRequest($"NÃO foi possivel atualiza o produto pois o Id={id} não existe");
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _context.Produtos.Update(produto);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var  produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);

                if (produto is null)
                {
                    return NotFound($"Produto com id={id} não encontrado");
                }

                _context.Remove(produto);
                await _context.SaveChangesAsync();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return Ok(produtoDto);

            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }
    }
}
