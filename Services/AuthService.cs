using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MongoDB.Driver;
using SoftCare.Dtos;
using SoftCare.Models;
using SoftCare.Repository;

namespace SoftCare.Services;

public class AuthService(TokenService tokenService, IAuthRepository authRepository) : IAuthService
{
    private readonly TokenService _tokenService = tokenService;
    private readonly IAuthRepository _authRepository = authRepository;

    public async Task<AuthResult> RegistrarAsync(RegisterRequest request)
    {
        var user = await _authRepository.BuscarUserAsync(request.email);

        if (user != null)
        {
            return new AuthResult(false, null, ["Email já utilizado"]);
        }
        
        var novoUser = new User(request.email, request.senha);

        var hashSenha = GerarHashSenhaComSalt(novoUser.Senha);
        novoUser.Senha = hashSenha;
        
        _authRepository.RegistrarAsync(novoUser);

        return new AuthResult(true, null, Enumerable.Empty<string>());
    }

    
    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
       var user = await _authRepository.BuscarUserAsync(request.email);

       if (user == null)
       {
           return new AuthResult(false, null, ["Email ou senha inválidos."]);
       }

       var senhaValida = VerificarSenha(user.Senha, request.senha);
       if (!senhaValida)
       {
           return new AuthResult(false, null, ["Email ou senha inválidos."]);
       }

       var token = _tokenService.GeradorToken(user);
        
       return new AuthResult(true, token, []);
    }

    private string GerarHashSenhaComSalt(string senha)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

        byte[] hash = KeyDerivation.Pbkdf2(
            password: senha,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);

        string saltBase64 = Convert.ToBase64String(salt);
        string hashBase64 = Convert.ToBase64String(hash);
        
        return $"{saltBase64}:{hashBase64}";
    }

    private bool VerificarSenha(string senhaHash, string senha)
    {
        try
        {
            var partes = senhaHash.Split(':');
            
            byte[] salt = Convert.FromBase64String(partes[0]);
            byte[] hashSalvo = Convert.FromBase64String(partes[1]);

            byte[] novoHash = KeyDerivation.Pbkdf2(
                password: senha,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );

            return CryptographicOperations.FixedTimeEquals(novoHash, hashSalvo);
        }
        catch { return false; }
        
    }
}