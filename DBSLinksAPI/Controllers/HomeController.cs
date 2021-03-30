using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBSLinksAPI.Models;
using DBSLinksAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DBSLinksAPI.Controllers
{
    [Route("api/v1/home")]
    public class HomeController: ControllerBase
    {
        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "GET - Anônimo";

        /*
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]User model)
        {
            string encryptedText = EncryptionServices.EncryptString(model.StringtoEncrypt);

            //var BackToPlainText = EncryptionServices.DecryptAsync(cypherObj.CypherText, cypherObj.IV);


            // Recupera o usuário
            //var user = UserRepository.Get(model.Username, model.Role, model.ComputerName);

            // Verifica se o usuário existe
            //if (user == null)
                //return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            //var token = TokenService.GenerateToken(user);

            // Oculta a senha
            //user.Password = "";

            // Retorna os dados
            return new
            {
                //user = encryptedText,
                //token = token

            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpPost]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public async Task<ActionResult> Employee([FromBody] CypherObj model)
        {
            string user = EncryptionServices.DecryptString(model.EncryptedString);
            return Ok( "Funcionário: " + user);
        }


        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";
        */
    }
}
