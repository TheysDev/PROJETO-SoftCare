using SoftCare.Dtos;
using SoftCare.Dtos.Auth;
using SoftCare.Models;

namespace SoftCare.Services;

public interface IAuthService
{
    public Task<AuthResult> RegistrarAsync(RegistroRequest request);
    public Task<AuthResult> LoginAsync(LoginRequest request);
}