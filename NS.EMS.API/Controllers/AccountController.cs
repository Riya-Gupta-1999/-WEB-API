using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NS.EMS.API.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NS.EMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login(Login login)
        {
            using (var contect=new EmployeeDbContext())
            {
                var user = contect.Login.Where(x => x.UserName == login.UserName && x.Password == login.Password && x.IsActive==true).FirstOrDefault();
                if(user!=null)
                {
                    var token = GenerateToken(user.UserName, user.Role);
                    return Ok(token);
                }
                else
                {
                    return Ok("Invalid username or password.");
                }
            }
            return Ok();
        }
        [NonAction]
        private string GenerateToken(string userName, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim (ClaimTypes.Name, userName),    new Claim (ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires:DateTime.Now.AddMinutes(30),
                signingCredentials : credentials );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
