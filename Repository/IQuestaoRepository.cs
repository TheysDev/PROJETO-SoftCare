using SoftCare.Models;

namespace SoftCare.Repository;

public interface IQuestaoRepository
{ 
    public Task<QuestionBank> BuscarQuestaoPeloCodeAsync(string questionCode);
    public Task<QuestionBank> PagBuscarQuestaoPelaCaregoriaAsync(string category, string? lastId);
    public Task<QuestionBank> BuscarTodasQuestoesAsync(string? lastId);
    public Task<QuestionBank> BuscarQuestaoPeloIdAsync(string id);

}