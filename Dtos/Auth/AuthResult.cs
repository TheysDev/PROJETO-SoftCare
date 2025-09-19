namespace SoftCare.Dtos;

public record AuthResult(bool Sucesso, string? Token, IEnumerable<string> Erros);