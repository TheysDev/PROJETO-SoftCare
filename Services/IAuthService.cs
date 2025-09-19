using SoftCare.Dtos;
using SoftCare.Models;

namespace SoftCare.Services;

public interface IAuthService
{
    public Task<AuthResult> RegistrarAsync(RegisterRequest request);
    public Task<AuthResult> LoginAsync(LoginRequest request);
}