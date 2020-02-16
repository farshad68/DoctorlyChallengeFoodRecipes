using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Webservices.Data;
using Webservices.RequestModel.Account;

namespace Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;

        public AuthenticateController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {

                var authClaims =new ClaimsIdentity( new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName)
                });

                
                var roles =  userManager.GetRolesAsync(user).Result;
                if (roles.Count > 0)
                {
                    foreach (var role in roles) { authClaims.AddClaim(new Claim(ClaimTypes.Role, role)); }
                }


                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisISaLongandSecureSecureKeyDoYouBeliveThat"));

                var token = new JwtSecurityToken(
                    issuer: "http://doctorly.de",
                    audience: "http://doctorly.de",
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims.Claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );



                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}