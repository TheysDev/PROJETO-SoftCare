using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SoftCare.Models;

public class DailyEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("user_id")]
    public string UserId { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("date")]
    public DateTime CheckDate { get; set; }
    
    [BsonElement("answers")]
    public List<Answer> Answers { get; set; } = new List<Answer>();
}