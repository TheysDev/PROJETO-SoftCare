using FluentValidation;
using SoftCare.Dtos;


namespace SoftCare.Validations;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(e => e.email)
            .NotEmpty().WithMessage("O campo de e-mail é obrigatório.")
            .EmailAddress().WithMessage("Por favor, forneça um endereço de e-mail válido.");

        RuleFor(s => s.senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.");
    }
}