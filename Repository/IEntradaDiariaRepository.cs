using SoftCare.Dtos.Analise;
using SoftCare.Models;
using SoftCare.Retornos;

namespace SoftCare.Repository;

public interface IEntradaDiariaRepository
{
    public Task<Result<string>> RegistrarEntradaDiariaAsync(DailyEntry check);
    
    public Task<List<ItemAnalise>> MontarAnaliseComCategoriaAsync(string userId, string category, DateTime startDate, DateTime endDate);
}