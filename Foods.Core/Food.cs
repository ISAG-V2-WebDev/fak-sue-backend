using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Foods.Core
{
    public class Food
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Restaurant { get; set; }
        public string FoodName { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
    }
}

