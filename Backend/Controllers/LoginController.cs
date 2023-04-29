using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Config;
using Backend.Models;
using Backend.Models.Request;
using Backend.Services;
using Backend.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IMongoCollection<User> _user;
    private readonly ILogger<UserController> _logger;
    
    public LoginController(ILogger<UserController> logger, IDbClient dbClient)
    {
        _user = dbClient.UserCollection();
        _logger = logger;
    }
    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(RegisterAdminRequest loginDTO)
    {
        try
        {
            if (string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
                return BadRequest("Username and/or Password not specified");

            User user = await _user.Find(x =>
                x.Username == loginDTO.Username && PasswordEncryption.Encrypt(loginDTO.Password) == x.Password &&
                !x.Banned && !x.Deleted).FirstOrDefaultAsync();
            if (user != null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.StaticConfig["Jwt:SecretKey"]!));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    //issuer: "ABCxyz",
                    //audience: "http://localhost:7150",
                    claims: new List<Claim>{
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                    },
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signinCredentials);
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
            }
        }
        catch
        {
            return BadRequest("An error occured in generating the token");
        }
        return Unauthorized("You are not authorized");
    }
}