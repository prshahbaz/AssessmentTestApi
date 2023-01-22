using AssessmentApp.Model;
using AssessmentApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssessmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config, AssessmentDbContext context) : base(context)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("GetToken")]
        public IActionResult Token(string UserName, string Password)
        {


            IActionResult response = Unauthorized();
            var user = AuthenticateUser(UserName, Password);

            if (user != null)
            {
               // var tokenString = GenerateJSONWebToken(user);
                var tokenString = GenerateJWTToken(UserName, Password);
                response = Ok(new { token = tokenString });
            }

            return response;
        }
        private string GenerateJWTToken(string UserName, string Password)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, UserName),
                new Claim(JwtRegisteredClaimNames.Email, UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return (stringToken);
        }
      
        private UserModel AuthenticateUser(string UserName, string Password)
        {
            var userData= _context.Users.Where(o => o.UserName == UserName && o.Password ==Password).FirstOrDefault();
            UserModel model = new UserModel();
            //Validate the User Credentials

            if (userData!=null)
            {
                model = new UserModel { Username = userData.UserName, Date = DateTime.Now };
            }
            return model;
        }
    }
}
