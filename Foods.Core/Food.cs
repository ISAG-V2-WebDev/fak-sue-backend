using MongoDB.Bson.Serialization.Attributes;

namespace Foods.Core
{
    public class Food
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public string Restaurant { get; set; }
        public string FoodName { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
    }
}

