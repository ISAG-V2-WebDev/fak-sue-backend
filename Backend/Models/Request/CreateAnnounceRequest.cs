using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class CreateAnnounceRequest
{
    [Required]
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
}