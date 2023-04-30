using System.Text;
using Backend.Config;
using Backend.Controllers;
using Backend.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
        ClockSkew = TimeSpan.Zero
    };
});
    // .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => builder.Configuration.Bind("JwtSettings", options))
    // .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options));
    // .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
Console.WriteLine(builder.Configuration["CONNECTION_STRING"]);
Console.WriteLine(builder.Configuration["ADMIN_CONNECTION_STRING"]);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(name: builder.Configuration["Authorization:PolicyName"]!,
        policy =>
        {
            policy.RequireRole("admin");
        });
});

builder.Services.AddCors(option =>
{
    option.AddPolicy(name: builder.Configuration["Cors:PolicyName"]!,
        policy =>
        {
            policy.WithOrigins("http://localhost:7150", "http://localhost:5101")
                .SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

//------------------------------------------------------------------------------------------

builder.Services.Configure<FoodsDB_config>(builder.Configuration);
builder.Services.AddSingleton<IDbClient, DbClient>();
builder.Services.AddTransient<IMenuServices, MenuServices>();
builder.Services.AddSingleton<IBlogServices, BlogServices>();
builder.Services.AddSingleton<IUserServices, UserServices>();
builder.Services.AddSingleton(new Configuration(builder.Configuration));

//------------------------------------------------------------------------------------------

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    // builder.Services.AddAuthentication("BasicAuthentication")
    //     .AddScheme<AuthenticationSchemeOptions, BasicAuthenHandler>("BasicAuthentication", null);
    // builder.Services.AddSingleton<IUserService, LoginController>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder.Configuration["Cors:PolicyName"]!);
app.UseRevokeToken();

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();