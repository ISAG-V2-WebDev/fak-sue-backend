using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Backend.Models.Request;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Backend.Middlewares;

public class BasicAuthenHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    readonly IUserService _userService;
    
    public BasicAuthenHandler(IUserService userService,  
        IOptionsMonitor<AuthenticationSchemeOptions> options,  
        ILoggerFactory logger,  
        UrlEncoder encoder,  
        ISystemClock clock)  
        : base(options, logger, encoder, clock)  
    {  
        _userService = userService;  
    }  

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()  
    {  
        string username = null;  
        try  
        {  
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);  
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');  
            username = credentials.FirstOrDefault();  
            var password = credentials.LastOrDefault();

            RegisterAdminRequest loginDTO = new RegisterAdminRequest();
            loginDTO.Username = username;
            loginDTO.Password = password;
            
            if (!_userService.ValidateCredentials(loginDTO))  
                throw new ArgumentException("Invalid credentials");  
        }  
        catch (Exception ex)  
        {  
            return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");  
        }  

        var claims = new[] {  
            new Claim(ClaimTypes.Name, username)  
        };  
        var identity = new ClaimsIdentity(claims, Scheme.Name);  
        var principal = new ClaimsPrincipal(identity);  
        var ticket = new AuthenticationTicket(principal, Scheme.Name);  

        return AuthenticateResult.Success(ticket);  
    }
}