using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models
{
    [BsonIgnoreExtraElements]
    public class Menu
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement("restaurant")]
        public string Restaurant { get; set; } = "";
        [BsonElement("foodname")]
        public string FoodName { get; set; } = "";
        [BsonElement("category")]
        public string Category { get; set; } = "";
        [BsonElement("price")]
        public double Price { get; set; } = 0;
    }
}

