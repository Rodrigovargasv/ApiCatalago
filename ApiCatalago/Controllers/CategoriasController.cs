using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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



        
        
       
    }
}

    
