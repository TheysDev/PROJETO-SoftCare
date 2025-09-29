using SoftCare.Dtos.Analise;
using SoftCare.Dtos.Respostas;
using SoftCare.Models;
using SoftCare.Repository;
using SoftCare.Retornos;

namespace SoftCare.Services;

public class EntradaDiariaService(IEntradaDiariaRepository entradaDiariaRepository)
    : IEntradaDiariaService
{
    public async Task<Result<string>> RegistrarEntradaDiariaAsync(List<RespostasRequest> request, string userId)
    {
        var answers = request.Select(req => new Answer
            {
                QuestionCode = req.QuestionCode,
                QuestionText = req.QuestionText,
                Type = req.Type,
                Value = req.Value,
                AnsweredAt = DateTime.UtcNow
            })
            .ToList();

        var newEntry = new DailyEntry
        {
            UserId = userId,
            CheckDate = DateTime.UtcNow,
            Answers = answers
        };

        return await entradaDiariaRepository.RegistrarEntradaDiariaAsync(newEntry);
    }

    public async Task<RespostaAnalise> GerarAnalisePelaCategoriaAsync(string userId, string category, DateTime startDate,
        DateTime endDate)
    {
        var analiseBruta =
            await entradaDiariaRepository.MontarAnaliseComCategoriaAsync(userId, category, startDate, endDate);

        if (analiseBruta == null || !analiseBruta.Any())
        {
            return new RespostaAnalise();
        }
        var contagem = analiseBruta.Sum(item => item.Contagem);

        var analiseConsolidada = analiseBruta.Select(item => item with
            {
                Porcentagem = Math.Round((double)item.Contagem / contagem * 100, 2)
            }).OrderByDescending(item => item.Porcentagem)
            .ToList();
        
        var resultadoPrincipal = analiseConsolidada.FirstOrDefault()?.Resposta ?? "Indefinido";
        var mainPercentage = analiseConsolidada.FirstOrDefault()?.Porcentagem ?? 0;

        var level = PegarLevelDaPercentagem(mainPercentage);

        return new RespostaAnalise
        {
            Consolidado = analiseConsolidada,
            ResultadoPrincipal = resultadoPrincipal,
            Nivel = level
        };
    }
    
    private string PegarLevelDaPercentagem(double percentage)
    {
        if (percentage <= 25) return "Neutro";
        if (percentage <= 50) return "Leve";
        if (percentage <= 75) return "Moderado";
        return "Agudo";
    }
    
    
}