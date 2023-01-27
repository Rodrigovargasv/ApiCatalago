using ApiCatalago.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        [HttpGet]
        public string listaProduto()
        {
            var name = "";
            var produtos = new List<string>();
            produtos.Add("das");

            produtos.Add("Rodrigo");
            foreach (var produto in produtos)
            {
                name = produto.ToString();

            }
            return name;

        }
        [HttpGet]
        public string listaProduto1()
        {
            var name = "";
            var produtos = new List<string>();
            produtos.Add("das");


            foreach (var produto in produtos)
            {
                name = produto.ToString();

            }
            return name;

        }
    }
}

    
