using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SoftCare.Data;
using SoftCare.Dtos;
using SoftCare.Dtos.User;
using SoftCare.Models;

namespace SoftCare.Repositorios;

public class AuthRepository : IAuthRepository
{
    private readonly IMongoCollection<User> _userCollection;

    private const string COLLECTIONNAME = "users";
    
    public AuthRepository(IOptions<MongoDBConfig> mongoDbSettings) {
        
        var client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
        var database = client.GetDatabase(mongoDbSettings.Value.NomeDatabase);
        _userCollection = database.GetCollection<User>(COLLECTIONNAME);
    }

    public async void  RegistrarAsync(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task<User> BuscarUserAsync(string email)
    {
        return await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
    }
}