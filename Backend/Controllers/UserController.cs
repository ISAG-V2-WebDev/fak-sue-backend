using System.Security.Claims;
using Backend.Models;
using Backend.Models.Request;
using Backend.Models.Response;
using Backend.Services;
using Backend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class UserController : ControllerBase
{
    private readonly IMongoCollection<User> _user;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IDbClient dbClient)
    {
        _user = dbClient.UserCollection();
        _logger = logger;
    }

    [HttpGet]
    [Route("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        
        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");

        UserResponse userResponse = new UserResponse(user);

        return Ok(userResponse);
    }

    [HttpPatch]
    [Route("resetpassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest body)
    {
        if (body.Password != body.ConfirmPassword)
            return Unauthorized("Password does not match.");
        
        string? username = Request.HttpContext.User.FindFirstValue("username");
        
        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");

        string encryptedPass = PasswordEncryption.Encrypt(body.Password);
        if (encryptedPass == user.Password)
            return BadRequest("New password is the same as current password.");

        user.Password = encryptedPass;

        await _user.ReplaceOneAsync(x => x.Username == username, user);

        return Ok($"Password for {user} reset.");
    }

    // public Task<IActionResult> UpdateUser(UpdateUserRequest body)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<IActionResult> DeleteUser()
    // {
    //     throw new NotImplementedException();
    // }
}