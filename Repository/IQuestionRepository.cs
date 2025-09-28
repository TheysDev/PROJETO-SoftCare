using SoftCare.Enum;
using SoftCare.Models;

namespace SoftCare.Repository;

public interface IQuestionRepository
{ 
    public Task<QuestionBank> GetQuestionBankCodeAsync(string questionCode);
    public Task<QuestionBank> GetNextQuestionBankCategoryAsync(string category, string? lastId);
    public Task<QuestionBank> GetAllQuestionBankAsync(string? lastId);

}