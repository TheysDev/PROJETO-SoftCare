using SoftCare.Models;

namespace SoftCare.Repository;

public interface IAuthRepository
{
    public void RegistrarAsync(User user);

    public Task<User> BuscarUserAsync(string email);
}