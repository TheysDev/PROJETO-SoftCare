using SoftCare.Dtos.Respostas;

namespace SoftCare.Validations;

public class PerguntasValidator : AbstractValidator<RespostasRequest>
{
    public PerguntasValidator()
    {
        RuleFor(qc => qc.QuestionCode)
            .NotEmpty().WithMessage("Question Code é obrigatório.");
        RuleFor(qt => qt.QuestionText)
            .NotEmpty().WithMessage("Question Text é obrigatório.");
        RuleFor(t => t.Type)
            .NotEmpty().WithMessage("Question Type é obrigatório.");
        RuleFor(v=> v.Value)
            .NotEmpty().WithMessage("Value é obrigatório.");
    }
}