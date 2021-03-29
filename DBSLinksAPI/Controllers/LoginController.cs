
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DBSLinksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DBSLinksAPI.Services;
using DBSLinksAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace DBSLinksAPI.Controllers
{
    [ApiController]
    [Route("api/v1/login")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public LoginController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Login>> Authenticate([FromBody] Login model)
        {

            if (!ModelState.IsValid)
            {
                LoginHistoryService.WriteToLoginHistory(model, "Failed", "BadRequest", _db);
                return BadRequest(ModelState);
            }
            string AesKey = _configuration.GetValue<string>("Services:AesKey");

            //string encryptedComputerName = EncryptionServices.EncryptString(model.ComputerName, AesKey);
            //string unencryptedUserName = EncryptionServices.DecryptString(encryptedComputerName, AesKey);


            if (model.UserName == null || model.ComputerName == null)
            {
                LoginHistoryService.WriteToLoginHistory(model, "Failed", "Username or ComputerName is null", _db);
                return Unauthorized();
            }
            
            var loginDetails = await _db.Logins
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.UserName == model.UserName
                                               && u.ComputerName == model.ComputerName);

            if (loginDetails == null)
            {
                LoginHistoryService.WriteToLoginHistory(model, "Failed", "Username or ComputerName could not found in database", _db);
                return Unauthorized();
            }

            LoginHistoryService.WriteToLoginHistory(model, "Successful", "Ok", _db);

            string tokenKey = _configuration.GetValue<string>("Services:TokenKey");
            var token = TokenService.GenerateToken(loginDetails, tokenKey);

            return Ok(token);

        }
    }
}
