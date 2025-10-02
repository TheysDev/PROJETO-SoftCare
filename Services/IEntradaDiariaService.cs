using SoftCare.Dtos.Questoes;
using SoftCare.Dtos.Respostas;
using SoftCare.Retornos;

namespace SoftCare.Services;

public interface IEntradaDiariaService
{
    Task<Result<string>> RegistrarEntradaDiariaAsync(List<RespostasRequest> request, string userId);
    Task<List<ResumoRespostasDto>> ResumoDasDezUltimasEntradasDoUsuarioAsync(string userId);
    Task<List<ResultadoCategoriaDto>> ResumoDasDezUltimasEntradasPorCategoriaAsync(string userId);
}