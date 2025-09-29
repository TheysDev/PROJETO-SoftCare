using SoftCare.Dtos.Analise;
using SoftCare.Dtos.Respostas;
using SoftCare.Retornos;

namespace SoftCare.Services;

public interface IEntradaDiariaService
{
    public Task<Result<string>> RegistrarEntradaDiariaAsync(List<RespostasRequest> request, string userId);
    public Task<RespostaAnalise> GerarAnalisePelaCategoriaAsync(string userId, string category, DateTime startDate, DateTime endDate);
}