using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class EditOrderRequest
{
    [JsonPropertyName("menu")] 
    public Menu? Menu { get; set; }

    [JsonPropertyName("detail")] 
    public string? Detail { get; set; }

    [JsonPropertyName("quantity")] 
    public int? Quantity { get; set; }
}