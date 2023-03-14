using ApiCatalago.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        // Definando instancias do identity Manager
        private readonly UserManager<IdentityUser> _UserManager;

        private readonly SignInManager<IdentityUser> _SignInManager;

        // injetando instancias de UserManager e SignInManager via construtor.
        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
        }


        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acesso em : "
                + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            /*
                if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
              }

              */
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _UserManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _SignInManager.SignInAsync(user, false);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            // verifica se o model é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            // verifica as crendencias do usuário e retorna um valor
            var result = await _SignInManager.PasswordSignInAsync(
                userInfo.Email, userInfo.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido...");
                return BadRequest(ModelState);
            }
        }
    }
}
