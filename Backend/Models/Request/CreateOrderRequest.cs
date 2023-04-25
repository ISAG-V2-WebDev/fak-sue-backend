using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class CreateOrderRequest
{
    [Required] 
    [JsonPropertyName("user")] 
    public User User { get; set; } = null!;
    
    [Required] 
    [JsonPropertyName("menu")] 
    public Menu Menu { get; set; } = null!;
    
    [JsonPropertyName("detail")] 
    public string Detail { get; set; } = "";
    
    [Required] 
    [JsonPropertyName("quantity")] 
    public int Quantity { get; set; } = 0;

    [Required]
    [JsonPropertyName("blog_id")]
    public string BlogId { get; set; } = "";
}