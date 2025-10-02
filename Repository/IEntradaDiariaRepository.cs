using SoftCare.Dtos.Questoes;
using SoftCare.Dtos.Respostas;
using SoftCare.Models;
using SoftCare.Retornos;

namespace SoftCare.Repository;

public interface IEntradaDiariaRepository
{ 
    Task<Result<string>> RegistrarEntradaDiariaAsync(DailyEntry check);
    Task<List<ResumoRespostasDto>> BuscarDezUltimasEntradasAsync(string userId);
    Task<List<ResumoCategoriaDto>> ResumoDasDezUltimasEntradasPorCategoriaAsync(string userId);
}