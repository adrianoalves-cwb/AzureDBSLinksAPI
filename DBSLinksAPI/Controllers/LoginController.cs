
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

namespace DBSLinksAPI.Controllers
{
    [ApiController]
    [Route("v1/login")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext db)
        {
            _db = db;
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

            //string unencryptedUserName = EncryptionServices.DecryptString(model.UserName);
            //string encryptedComputerName = EncryptionServices.EncryptString(model.ComputerName);

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
            var token = TokenService.GenerateToken(loginDetails);
            return Ok(token);

        }
    }
}
