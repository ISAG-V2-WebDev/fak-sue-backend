using System.Security.Claims;
using Backend.Models;
using Backend.Models.Request;
using Backend.Models.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class BlogController : ControllerBase
{
    private readonly IMongoCollection<Blog> _blog;
    private readonly IMongoCollection<User> _user;
    private readonly ILogger<BlogController> _logger;

    public BlogController(ILogger<BlogController> logger, IDbClient dbClient)
    {
        _blog = dbClient.BlogCollection();
        _user = dbClient.UserCollection();
        _logger = logger;
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("list")]
    public async Task<IActionResult> GetBlogs()
    {
        List<Blog> blogs = await _blog.Find(x => !x.Hide && !x.Deleted).ToListAsync();

        List<BlogResponse> blogResponses = new List<BlogResponse>();

        foreach (Blog blog in blogs)
        {
            User? user = await _user.Find(x => x.Id == blog.UserId && !x.Banned && !x.Deleted)
                .FirstOrDefaultAsync();
            BlogResponse blogResponse = new BlogResponse(blog, user);
            blogResponses.Add(blogResponse);
        }

        BlogListResponse blogList = new BlogListResponse
            { Blogs = blogResponses.OrderByDescending(x => x.CreatedDate).ToList() };
        
        return Ok(blogList);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("id={id:length(24)}")]
    public async Task<IActionResult> GetBlog(string id)
    {
        Blog? blog = await _blog.Find(x => x.Id == id && !x.Hide && !x.Deleted).FirstOrDefaultAsync();

        if (blog == null)
            return NotFound("Blog is not found.");
        
        User? author = await _user.Find(x => x.Id == blog.UserId && !x.Banned && !x.Deleted).FirstOrDefaultAsync();

        BlogResponse blogResponse = new BlogResponse(blog, author);
        return Ok(blogResponse);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateBlog(CreateBlogRequest body)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        if (String.IsNullOrEmpty(username))
            return Unauthorized("You are not authorized user.");

        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found");
        
        Blog newBlog = new Blog { Topic = body.Topic, Detail = body.Content, UserId = user.Id, TimeStamp = body.TimeStamp, 
            Orders = body.Orders, MaxOrder = body.MaxOrder};
        await _blog.InsertOneAsync(newBlog);

        return Ok(CreatedAtAction(nameof(CreateBlog), newBlog));
    }
    
    [HttpPatch]
    [Route("update/id={id:length(24)}")]
    public async Task<IActionResult> UpdateBlog(string id, EditContentRequest body)
    {
        Blog? blog = await _blog.Find(x => x.Id == id && !x.Hide && !x.Deleted).FirstOrDefaultAsync();

        if (blog == null)
            return NotFound("Blog is not found.");
        
        blog.Topic = body.Topic ?? blog.Topic;
        blog.Detail = body.Content ?? blog.Detail;
        blog.TimeStamp = body.TimeStamp;
        blog.Orders = body.Orders;
        blog.MaxOrder = body.MaxOrder;
        blog.Hide = body.Hide ?? blog.Hide;
        blog.UpdatedDate = DateTime.UtcNow;
        
        await _blog.ReplaceOneAsync(x => x.Id == id, blog);

        return Ok(blog);
    }
    
    [HttpPatch]
    [Route("hide/id={id:length(24)}")]
    public async Task<IActionResult> HideBlog(string id)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        if (String.IsNullOrEmpty(username))
            return Unauthorized("You are not authorized user.");
        
        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found");
        
        Blog? blog = await _blog.Find(x => x.Id == id && !x.Hide && !x.Deleted).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found");
        
        if (user.Id != blog.UserId)
            return Unauthorized("You are not the author of this blog");
        
        blog.Hide = !blog.Hide;

        await _blog.ReplaceOneAsync(x => x.Id == id, blog);
        
        return Ok(blog);
    }
    
    [HttpDelete]
    [Route("delete/id={id:length(24)}")]
    public async Task<IActionResult> DeleteBlog(string id)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        if (String.IsNullOrEmpty(username))
            return Unauthorized("You are not authorized user.");

        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found");
        
        Blog? blog = await _blog.Find(x => x.Id == id && !x.Hide && !x.Deleted).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found");
        
        if (user.Id != blog.UserId)
            return Unauthorized("You are not the author of this blog");
        
        blog.Hide = true;
        blog.Deleted = true;
        
        await _blog.ReplaceOneAsync(x => x.Id == id, blog);
        
        return NoContent();
    }
}