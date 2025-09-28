using SoftCare.Dtos.Questions;
using SoftCare.Enum;
using SoftCare.Retornos;

namespace SoftCare.Services;

public interface IQuestionService
{
    public Task<Result<QuestionDoFront>> GetQuestionDoFrontCode(string questionCode);
    public Task<Result<QuestionDoFront>> GetNextQuestionDoFrontCategory(string category, string? lastId);
    public Task<Result<QuestionDoFront>> GetAllQuestionDoFront(string? lastId);
    
}