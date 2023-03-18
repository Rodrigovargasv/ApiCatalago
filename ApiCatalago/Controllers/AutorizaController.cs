using ApiCatalago.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCatalago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        // Definando instancias do identity Manager
        private readonly UserManager<IdentityUser> _UserManager;

        private readonly SignInManager<IdentityUser> _SignInManager;

        // Definando inestancia de Iconfiguration
        private readonly IConfiguration _configuration;


        // injetando instancias de UserManager e SignInManager, ICofiguration via construtor.
        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _configuration = configuration;
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
            return Ok(GeraToken(model));
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
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido...");
                return BadRequest(ModelState);
            }
        }

        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            // Define declarações do usuário

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("arya", "gato"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            // gera uma chave com base em um algoritimo simetrico
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConstants:Key"]));

            // gera a assinatura digital do token susando o algoritimo Hmac e a chave privada
            var credencias = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            // tempo de expiração do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token;
            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credencias

            );

            // retorna os dados com o token e informações
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }

    }
}
