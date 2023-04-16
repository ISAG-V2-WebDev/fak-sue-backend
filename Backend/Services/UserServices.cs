using Backend.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services;

public class UserServices : IUserServices
{
    public Task<IActionResult> GetUserProfile()
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> ResetPassword(ResetPasswordRequest body)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> UpdateUser(UpdateUserRequest body)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> DeleteUser()
    {
        throw new NotImplementedException();
    }
}