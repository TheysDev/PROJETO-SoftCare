using SoftCare.Dtos.Questions;
using SoftCare.Enum;
using SoftCare.Repository;
using SoftCare.Retornos;

namespace SoftCare.Services;

public class QuestionService(IQuestionRepository repository) : IQuestionService
{
    public async Task<Result<QuestionDoFront>> GetQuestionDoFrontCode(string questionCode)
    {
        var question = await repository.GetQuestionBankCodeAsync(questionCode);

        if (question == null)
        {
            return Result<QuestionDoFront>.Fail("Questão não encontrada");
        }

        var frontQuestion = new QuestionDoFront(question.Id,question.QuestionText, question.Options);

        return Result<QuestionDoFront>.Ok(frontQuestion);
    }

    public async Task<Result<QuestionDoFront>> GetNextQuestionDoFrontCategory(string category, string? lastId)
    {
        var question = await repository.GetNextQuestionBankCategoryAsync(category, lastId);

        if (question == null)
        {
            return null;
        }
        var frontQuestion = new QuestionDoFront(question.Id,question.QuestionText, question.Options);
        
        return Result<QuestionDoFront>.Ok(frontQuestion);
    }

    public async Task<Result<QuestionDoFront>> GetAllQuestionDoFront(string? lastId)
    {
        var question = await repository.GetAllQuestionBankAsync(lastId);

        if (question == null)
        {
            return null;
        }
        var frontQuestion = new QuestionDoFront(question.Id,question.QuestionText, question.Options);
        
        return Result<QuestionDoFront>.Ok(frontQuestion);
    }
}