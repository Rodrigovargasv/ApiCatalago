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
    
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // Define os tipos de retorno mais commum e códigos de status retornado para cada tipo de metado action deste controller
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProdutosController : ControllerBase
    {
        // Instância a conexão com banco de dados
        private readonly ApiCatalagoDbContext _context;

        // Injetando serviço via construtor
        private readonly IMapper _mapper;

        // Injeta a dependencia via construtor
        public ProdutosController(ApiCatalagoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Retorna todos os produtos da tabela produtos
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


        // Retorna o produto com id especificado no parametro
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



        // Inseri um produto na tabela produtos
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

        // Atualiza o produto do id passado por parametro
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


         // Deleta o produto do ID passado por parametro
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
