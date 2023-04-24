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
// [Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly IMongoCollection<User> _user;
    private readonly IMongoCollection<Blog> _blog;
    private readonly IMongoCollection<Menu> _menu;
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger, IDbClient dbClient)
    {
        _user = dbClient.UserCollection();
        _blog = dbClient.BlogCollection();
        _menu = dbClient.MenuCollection();
        _logger = logger;
    }

    [HttpGet]
    [Route("manage/user/list")]
    public async Task<IActionResult> GetUserList()
    {
        List<User> users = await _user.Find(x => true).ToListAsync();
        return Ok(users.OrderByDescending(x => x.UpdatedDate).ToList());
    }

    [HttpGet]
    [Route("manage/user/get/id={id:length(24)}")]
    public async Task<IActionResult> GetUser_id(string id)
    {
        User? user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");

        return Ok(user);
    }

    [HttpPatch]
    [Route("manage/user/update/id={id:length(24)}")]
    public async Task<IActionResult> UpdateUser_id(string id, [FromBody] AdminUserUpdateRequest body)
    {
        User? user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");
        
        if (body.Password != null)
            user.Password = PasswordEncryption.Encrypt(body.Password);
        
        user.Username = body.Username ?? user.Username;
        user.Role = body.Role ?? user.Role;
        user.Name = body.Name ?? user.Name;
        user.StudentId = body.StudentId ?? user.StudentId;
        user.Banned = body.Banned ?? user.Banned;
        user.Deleted = body.Deleted ?? user.Deleted;
        user.ProfileImage = body.ProfileImage ?? user.ProfileImage;
        user.UpdatedDate = DateTime.UtcNow;

        await _user.ReplaceOneAsync(x => x.Id == id, user);
        return Ok(user);
    }

    [HttpPatch]
    [Route("manage/user/banned/id={id:length(24)}")]
    public async Task<IActionResult> BannedUser_id(string id)
    {
        User? user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");

        user.Banned = !user.Banned;

        await _user.ReplaceOneAsync(x => x.Id == id, user);
        return Ok(user);
    }

    [HttpPatch]
    [Route("manage/user/delete/id={id:length(24)}")]
    public async Task<IActionResult> DeleteUser_id(string id)
    {
        User? user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found.");

        user.Banned = !user.Deleted;
        user.Deleted = !user.Deleted;

        await _user.ReplaceOneAsync(x => x.Id == id, user);
        return Ok(user);
    }

    [HttpGet]
    [Route("manage/blog/list")]
    public async Task<IActionResult> GetBlogs()
    {
        List<Blog> blogs = await _blog.Find(x => true).ToListAsync();
        List<AdminBlogResponse> blogResponses = new List<AdminBlogResponse>();

        foreach (Blog blog in blogs)
        {
            User user = await _user.Find(x => x.Id == blog.UserId).FirstAsync();
            AdminBlogResponse blogResponse = new AdminBlogResponse(blog);
            blogResponses.Add(blogResponse);
        }

        AdminBlogListResponse blogList = new AdminBlogListResponse
            { Blogs = blogResponses.OrderByDescending(x => x.UpdatedDate).ToList() };

        return Ok(blogList);
    }

    [HttpPatch]
    [Route("manage/blog/update/id={id:length(24)}")]
    public async Task<IActionResult> UpdateBlog_id(string id, [FromBody] AdminBlogUpdateRequest body)
    {
        Blog? blog = await _blog.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found.");

        blog.Topic = body.Topic ?? blog.Topic;
        blog.Detail = body.Content ?? blog.Detail;
        blog.Hide = body.Hide ?? blog.Hide;
        blog.Deleted = body.Delete ?? blog.Deleted;
        blog.UpdatedDate = DateTime.UtcNow;

        await _blog.ReplaceOneAsync(x => x.Id == id, blog);
        return Ok(blog);
    }

    [HttpPatch]
    [Route("manage/blog/hide/id={id:length(24)}")]
    public async Task<IActionResult> HideBlog_id(string id)
    {
        Blog? blog = await _blog.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found.");

        blog.Hide = !blog.Hide;

        await _blog.ReplaceOneAsync(x => x.Id == id, blog);
        return Ok(blog);
    }
    
    [HttpPatch]
    [Route("manage/blog/delete/id={id:length(24)}")]
    public async Task<IActionResult> DeleteBlog_id(string id)
    {
        Blog? blog = await _blog.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found.");
        
        blog.Hide = !blog.Deleted;
        blog.Deleted = !blog.Deleted;

        await _blog.ReplaceOneAsync(x => x.Id == id, blog);
        return Ok(blog);
    }

    [HttpGet]
    [Route("setup/mock/data")]
    [AllowAnonymous]
    public async Task<IActionResult> MockData()
    {
        List<User>? newUsers = new List<User>
        {
            new User
            {
                Username = "admin1", Password = PasswordEncryption.Encrypt("AdminZa55+"), Name = "Admin Rosemarries",
                Email = "64010720@kmitl.ac.th",
                Role = "admin"
            },
            new User
            {
                Username = "admin2", Password = PasswordEncryption.Encrypt("AdminZa55+"), Name = "Admin PickAUserName",
                Role = "admin"
            },
            new User
            {
                Username = "admin3", Password = PasswordEncryption.Encrypt("AdminZa55+"), Name = "Admin Butler",
                Role = "admin"
            },
            new User
            {
                Username = "admin4", Password = PasswordEncryption.Encrypt("AdminZa55+"), Name = "Admin JanRainjer",
                Role = "admin"
            },
            new User
            {
                Username = "admin5", Password = PasswordEncryption.Encrypt("AdminZa55+"), Name = "Admin MuMu",
                Role = "admin"
            },
            new User { Username = "test", Password = PasswordEncryption.Encrypt("userX123"), Name = "Test" }
        };

        List<Blog>? newBlogs = new List<Blog>
        {
            new Blog
            {
                Topic = "Lunch",
                TimeStamp = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 12, 0, 0, DateTimeKind.Utc),
                UserId = newUsers[0].Id
            },
            new Blog
            {
                Topic = "Brunch Time!",
                TimeStamp = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 18, 0, 0, DateTimeKind.Utc),
                UserId = newUsers[1].Id
            },
            new Blog
            {
                Topic = "Lunch Time My Friend!",
                TimeStamp = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 12, 0, 0, DateTimeKind.Utc),
                UserId = newUsers[4].Id
            }
        };

        // await _user.InsertManyAsync(newUsers);
        await _blog.InsertManyAsync(newBlogs);

        return Ok(new { users = newUsers, blogs = newBlogs });
    }
}