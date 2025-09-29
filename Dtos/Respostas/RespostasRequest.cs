using SoftCare.Enum;

namespace SoftCare.Dtos.Respostas;

public record RespostasRequest(string QuestionCode, string QuestionText, QuestionType Type, int Value);