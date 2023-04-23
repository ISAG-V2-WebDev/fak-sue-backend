using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class CreateBlogRequest
{
    [Required]
    [JsonPropertyName("topic")]
    public string Topic { get; set; } = null!;

    [Required]
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
    
    [Required]
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    [JsonPropertyName("max_order")]
    public int MaxOrder { get; set; } = 1;
}