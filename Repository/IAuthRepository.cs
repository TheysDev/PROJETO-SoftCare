using SoftCare.Dtos;
using SoftCare.Dtos.User;
using SoftCare.Models;

namespace SoftCare.Repositorios;

public interface IAuthRepository
{
    public void RegistrarAsync(User user);

    public Task<User> BuscarUserAsync(string email);
}