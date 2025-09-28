using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SoftCare.Enum;

namespace SoftCare.Models;

public class QuestionBank
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public string Id { get; set; }
    
    [BsonElement("code")]
    [BsonRequired]
    public string QuestionCode { get; set; } = string.Empty;
    
    [BsonElement("text")]
    [BsonRequired]
    public string QuestionText { get; set; } = string.Empty;
    
    [BsonElement("type")]
    [BsonRequired]
    public QuestionType Type { get; set; }
    
    [BsonElement("options")]
    public List<QuestionOption> Options { get; set; } = [];
    
    [BsonElement("min")]
    [BsonIgnoreIfNull]
    public int? Min { get; set; }
    
    [BsonElement("max")]
    [BsonIgnoreIfNull]
    public int? Max { get; set; }
    
    [BsonElement("category")]
    [BsonRequired]
    public string Category { get; set; } = string.Empty;
    
    [BsonElement("ordering")]
    [BsonRequired]
    public double Ordering { get; set; }
    
    [BsonElement("is_active")]
    [BsonDefaultValue(true)] 
    public bool IsActive { get; set; }
}