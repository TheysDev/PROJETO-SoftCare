using MongoDB.Bson.Serialization.Attributes;

namespace SoftCare.Models;

public class QuestionOption
{
    [BsonElement("value")]
    [BsonRequired]
    public double Value { get; set; }
    
    [BsonElement("label")]
    [BsonRequired]
    public string Label { get; set; } = string.Empty;
}