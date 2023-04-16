using System.Security.Claims;
using Backend.Models;
using Backend.Models.Request;
using Backend.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Services;

public class BlogServices : IBlogServices
{
    private readonly IMongoCollection<Blog> _blog;
    private readonly IMongoCollection<User> _user;

    public BlogServices(IDbClient dbClient)
    {
        _blog = dbClient.BlogCollection();
        _user = dbClient.UserCollection();
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

    public async Task<BlogResponse> GetBlog(string id)
    {
        Blog? blog = await _blog.Find(x => x.Id == id && !x.Hide && !x.Deleted).FirstOrDefaultAsync();

        if (blog == null)
            return new BlogResponse(null!, null!);
        
        User? author = await _user.Find(x => x.Id == blog.UserId && !x.Banned && !x.Deleted).FirstOrDefaultAsync();

        BlogResponse blogResponse = new BlogResponse(blog, author);
        return blogResponse;
    }

    public async Task<BlogResponse> CreateBlog(CreateBlogRequest body, string username)
    {
        User? user = _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefault();
        if (user == null)
            return new BlogResponse(null!, null!);
        
        Blog newBlog = new Blog { Topic = body.Topic, Detail = body.Content, UserId = user.Id };
        await _blog.InsertOneAsync(newBlog);

        return new BlogResponse(newBlog, user);
    }

    public async Task<Blog?> UpdateBlog(string id, EditContentRequest body)
    {
        Blog? blog = await _blog.Find(x => x.Id == id && !x.Hide && !x.Deleted).FirstOrDefaultAsync();
        if (blog != null)
        {
            blog.Topic = body.Topic ?? blog.Topic;
            blog.Detail = body.Content ?? blog.Detail;
            blog.Hide = body.Hide ?? blog.Hide;
            blog.UpdatedDate = DateTime.UtcNow;
            await _blog.ReplaceOneAsync(x => x.Id == id, blog);
        }
        
        return blog;
    }

    public Task<BlogResponse> HideBlog(string id)
    {
        throw new NotImplementedException();
    }

    public Task<BlogResponse> DeleteBlog(string id)
    {
        throw new NotImplementedException();
    }
}