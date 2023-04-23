using System.Text.Json.Serialization;

namespace Backend.Models.Response;

public class BlogResponse
{
    [JsonPropertyName("blog_id")]
    public string BlogId { get; set; } = null!;
    
    [JsonPropertyName("author")]
    public UserResponse? Author { get; set; }
    
    [JsonPropertyName("topic")]
    public string Topic { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = "";
    
    [JsonPropertyName("max_order")] 
    public int MaxOrder { get; set; } = 1;
    
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    
    [JsonPropertyName("hide")]
    public bool Hide { get; set; }
    
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
    
    [JsonPropertyName("created_date")]
    public DateTime CreatedDate { get; set; }
    
    [JsonPropertyName("updated_date")]
    public DateTime UpdatedDate { get; set; }

    public BlogResponse(Blog blog, User? user)
    {
        if (blog != null)
        {
            this.BlogId = blog.Id;
            if (user != null)
                this.Author = new UserResponse(user);
            this.Topic = blog.Topic;
            this.Content = blog.Detail;
            this.MaxOrder = blog.MaxOrder;
            this.Timestamp = blog.TimeStamp;
            this.Hide = blog.Hide;
            this.Deleted = blog.Deleted;
            this.CreatedDate = blog.CreatedDate;
            this.UpdatedDate = blog.UpdatedDate;
        }
    }
}