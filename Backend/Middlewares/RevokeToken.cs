using System.IdentityModel.Tokens.Jwt;
using Backend.Config;
using Backend.Models;
using Backend.Services;
using Backend.Utilities;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Backend.Middlewares;

public class RevokeToken
{
    private readonly RequestDelegate _next;
    private readonly IMongoCollection<User> _user;
    public RevokeToken(RequestDelegate next, IDbClient dbClient)
    {
        _next = next;
        _user = dbClient.UserCollection();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check Token in request header Authorization section.
        string accessToken = context.Request.Headers.Authorization.ToString().Split(' ').Last();

        bool isValid = TokenUtils.ValidateToken(accessToken);

        if (isValid) { await _next(context); return; }

        string? refreshToken = context.Request.Cookies[Constant.Name.RefreshToken];

        if (String.IsNullOrEmpty(refreshToken)) { await _next(context); return; }

        bool isRefreshValid = TokenUtils.ValidateToken(refreshToken);

        if (!isRefreshValid) { await _next(context); return; }

        User? user = await DeserializeUser(refreshToken);

        if (user == null) { await _next(context); return; }

        string newAccessToken = TokenUtils.GenerateAccessToken(user);

        context.Request.Headers.Authorization = "Bearer " + newAccessToken;

        context.Response.Cookies.Append(Constant.Name.AccessToken, newAccessToken, new CookieOptions
        {
            HttpOnly = false,
            Expires = DateTime.UtcNow.AddDays(Constant.Number.AccessTokenExpiresInDay)
        });

        await _next(context);
    }

    private async Task<User> DeserializeUser(string token)
    {
        string data = new JwtSecurityTokenHandler().ReadJwtToken(token).Payload.SerializeToJson();
        // Console.WriteLine(data);

        JObject jsonData = JObject.Parse(data);

        User? user = await _user.Find(x => x.Username == jsonData["username"]!.ToString() && !x.Banned && !x.Deleted).FirstOrDefaultAsync();

        return user;
    }
}

public static class RevokeTokenExtensions
{
    public static IApplicationBuilder UseRevokeToken(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RevokeToken>();
    }
}