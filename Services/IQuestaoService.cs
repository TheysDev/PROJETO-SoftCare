using SoftCare.Dtos.Questoes;
using SoftCare.Retornos;

namespace SoftCare.Services;

public interface IQuestaoService
{
    public Task<Result<QuestaoResponse>> BuscarQuestaoPeloCodeAsync(string questionCode);
    public Task<Result<QuestaoResponse>> PagBuscarQuestaoPelaCategoryAsync(string category, string? lastId);
    public Task<Result<QuestaoResponse>> BuscarTodasAsQuestoesAsync(string? lastId);
    
}