using Backend.Models.Request;

namespace Backend.Services;

public interface IUserService
{
    bool ValidateCredentials(RegisterAdminRequest loginDTO);
}