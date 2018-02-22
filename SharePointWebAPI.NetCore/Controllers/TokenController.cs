using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.NetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class TokenController : Controller
    {
        // POST: api/Token
        [HttpPost]
        public IActionResult Create(string username, string password) => new ObjectResult(GenerateToken(username, password));
        private string GenerateToken(string username, string password)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Email, username)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789012345"));//a secret that needs to be at least 16 characters long
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "WebAPI.NetCore",
                audience: "Anybody",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}