using SoftCare.Dtos.Questoes;
using SoftCare.Enum;
using SoftCare.Repository;
using SoftCare.Retornos;

namespace SoftCare.Services;

public class QuestaoService(IQuestaoRepository repository) : IQuestaoService
{
    public async Task<Result<QuestaoResponse>> BuscarQuestaoPeloCodeAsync(string questionCode)
    {
        var question = await repository.BuscarQuestaoPeloCodeAsync(questionCode);

        if (question == null)
        {
            return Result<QuestaoResponse>.Fail("Questão não encontrada");
        }

        var frontQuestion = new QuestaoResponse(question.Id,question.QuestionCode,question.QuestionText,question.Type,question.Options);

        return Result<QuestaoResponse>.Ok(frontQuestion);
    }

    public async Task<Result<QuestaoResponse>> PagBuscarQuestaoPelaCategoryAsync(string category, string? lastId)
    {
        var question = await repository.PagBuscarQuestaoPelaCaregoriaAsync(category, lastId);

        if (question == null)
        {
            return null;
        }
        var frontQuestion = new QuestaoResponse(question.Id,question.QuestionCode,question.QuestionText,question.Type,question.Options);
        
        return Result<QuestaoResponse>.Ok(frontQuestion);
    }

    public async Task<Result<QuestaoResponse>> BuscarTodasAsQuestoesAsync(string? lastId)
    {
        var questao = await repository.BuscarTodasQuestoesAsync(lastId);

        if (questao == null)
        {
            return null;
        }
        var questaoResponse = new QuestaoResponse(questao.Id,questao.QuestionCode,questao.QuestionText,questao.Type,questao.Options);
        
        return Result<QuestaoResponse>.Ok(questaoResponse);
    }
}