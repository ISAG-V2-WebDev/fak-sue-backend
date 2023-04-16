using Backend.Models;
using Backend.Models.Request;
using Backend.Models.Response;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Services;

public class BlogServices : IBlogServices
{
    private readonly IMongoCollection<Blog> _blog;
    private readonly IMongoCollection<User> _user;
    private readonly ILogger<BlogServices> _logger;

    public BlogServices(IDbClient dbClient, ILogger<BlogServices> logger)
    {
        _blog = dbClient.BlogCollection();
        _user = dbClient.UserCollection();
        _logger = logger;
    }

    public async Task<BlogListResponse> GetBlogs()
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
        return blogList;
    }

    public Task<IActionResult> GetBlog(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> CreateBlog(CreateBlogRequest body)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> UpdateBlog(string id, EditContentRequest body)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> HideBlog(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> DeleteBlog(string id)
    {
        throw new NotImplementedException();
    }
}