using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SoftCare.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id  { get; set; }
    
    [BsonElement("email")] 
    public string Email { get; set; }
    
    [BsonElement("password_hash")]
    public string Senha { get; set; }
    
    [BsonElement("created_at")]
    public DateTime DataCricacao { get; set; }
    
    public User(string requestEmail, string hashSenha)
    {
        Email = requestEmail;
        Senha = hashSenha;
        DataCricacao = DateTime.UtcNow;
    }
}