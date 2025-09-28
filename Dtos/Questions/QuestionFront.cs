using SoftCare.Models;

namespace SoftCare.Dtos.Questions;

public record QuestionDoFront(string Id, string QuestionText, List<QuestionOption> Options);