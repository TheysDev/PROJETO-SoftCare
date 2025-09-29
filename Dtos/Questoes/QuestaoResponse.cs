using SoftCare.Enum;
using SoftCare.Models;

namespace SoftCare.Dtos.Questoes;

public record QuestaoResponse(string Id, string QuestionCode, string QuestionText, QuestionType Type, List<QuestionOption> Options);