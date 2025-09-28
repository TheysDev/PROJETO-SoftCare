using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SoftCare.Data;
using SoftCare.Models;

namespace SoftCare.Repository;

public class AuthRepository(IMongoDatabase database) : IAuthRepository
{
    private readonly IMongoCollection<User> _userCollection = database.GetCollection<User>(COLLECTIONNAME);

    private const string COLLECTIONNAME = "users";

    public async void RegistrarAsync(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task<User> BuscarUserAsync(string email)
    {
        return await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
    }
}