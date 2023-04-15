using Backend.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services;

public interface IUserServices
{
    public Task<IActionResult> GetUserProfile();
    public Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest body);
    public Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest body);
    public Task<IActionResult> DeleteUser();
}