using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("user")]
    public User User { get; set; } = null!;

    [BsonElement("menu")] 
    public Menu Menu { get; set; } = null!;

    [BsonElement("detail")] 
    public string Detail { get; set; } = "";

    [BsonElement("quantity")] 
    public int Quantity { get; set; } = 1;

    [BsonElement("blog_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BlogId { get; set; } = "";
}