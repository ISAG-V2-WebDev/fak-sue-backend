using System.Security.Claims;
using Backend.Config;
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

    [HttpPatch]
    [Route("update")]
    public async Task<IActionResult> UpdateUser(UpdateUserRequest body)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        
        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");

        user.Name = body.Name ?? user.Name;
        user.ProfileImage = body.ProfileImage ?? user.ProfileImage;
        user.UpdatedDate = DateTime.UtcNow;

        await _user.ReplaceOneAsync(x => x.Id == user.Id, user);

        return Ok(user);
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteUser()
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        
        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");

        user.Banned = !user.Deleted;
        user.Deleted = !user.Deleted;

        Response.Cookies.Delete(Constant.Name.AccessToken);
        Response.Cookies.Delete(Constant.Name.RefreshToken);

        await _user.ReplaceOneAsync(x => x.Id == user.Id, user);

        return Ok(user);
    }
}