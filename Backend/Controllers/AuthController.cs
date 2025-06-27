using backend.Model;
using Backend.Helpers;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext context;
        public AuthController(DataContext c)
        {
            context = c;
        }
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var user = context.Users.FirstOrDefault(u=>u.Username == username);
            if (user == null) return NotFound("No such user");
            if (!Hasher.Verify(password, user.SaltedPassword)) return Unauthorized("Invalid creds");
            var token = GenerateJwtToken(username, user.SaltedPassword);
            return Ok(new Dictionary<string, string>(){
                {"token",token},
                {"type",user.UserType.ToString()},
                {"name",username},
            });
        }

        [HttpPost("register")]
        public IActionResult Register(string username, string password,int type = 1)
        {
            var exists = context.Users.FirstOrDefault(u => u.Username == username);
            if (exists != null) return BadRequest("User already exists!");
            string salted = Hasher.Hash(password);
            context.Users.Add(new()
            {
                Username = username,
                SaltedPassword = salted,
                UserType = type
            });
            context.SaveChanges();
            var token = GenerateJwtToken(username, salted);
            return Ok(new Dictionary<string, string>(){
                {"token",token},
                {"type",type.ToString()},
                {"name",username}
            });
        }
        [HttpGet("all")]

        public IActionResult GetAllUsersDebug()
        {
            return Ok(context.Users.Include(u => u.PrivateNotes).ToList());
        }
        private string GenerateJwtToken(string username, string userId)
        {
            var claims = new List<Claim> {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Name, username),
    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is a secure key, requires a key size of at least '128' bits"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7295",
                audience: "https://localhost:7295",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
