using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SoftCare.Enum;

namespace SoftCare.Models;

public class Answer
{
    [BsonElement("question_code")]
    [BsonRequired]
    public string QuestionCode { get; set; } = string.Empty;
    
    [BsonElement("question_text")]
    [BsonRequired]
    public string QuestionText { get; set; } = string.Empty;
    
    [BsonElement("type")]
    [BsonRepresentation(BsonType.String)]
    public QuestionType Type { get; set; }
    
    [BsonElement("value")]
    public int Value { get; set; }
    
    [BsonElement("answered_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? AnsweredAt { get; set; }
}