using SoftCare.Dtos;

namespace SoftCare.Validations;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(e => e.email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Formato de e-mail inválido.");

        RuleFor(s => s.senha)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }
}