using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class AdminBlogUpdateRequest
{
    [JsonPropertyName("topic")]
    public string? Topic { get; set; }
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
    
    [JsonPropertyName("hide")]
    public bool? Hide { get; set; }
    
    [JsonPropertyName("delete")]
    public bool? Delete { get; set; }
}