using SoftCare.Dtos.Questoes;
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
        var perguntas = request.Select(req => new Answer
            {
                QuestionCode = req.QuestionCode,
                QuestionText = req.QuestionText,
                Type = req.Type,
                Value = req.Value,
                AnsweredAt = DateTime.UtcNow
            })
            .ToList();

        var novaEntrada = new DailyEntry
        {
            UserId = userId,
            CheckDate = DateTime.UtcNow,
            Answers = perguntas
        };

        return await entradaDiariaRepository.RegistrarEntradaDiariaAsync(novaEntrada);
    }
    public async Task<List<ResumoRespostasDto>> ResumoDasDezUltimasEntradasDoUsuarioAsync(string userId)
    {
        var resumo = await entradaDiariaRepository.BuscarDezUltimasEntradasAsync(userId);

        return resumo;
    }
    private record DefinicaoLevel(string Nivel, double FaixaMin, double FaixaMax);
    private static Dictionary<string, List<DefinicaoLevel>> IniciarDefinicaoEscala()
    {
        var escalaRiscos = new List<DefinicaoLevel>
        {
            new("Neutro", 0, 25), new("Leve", 26, 50),
            new("Moderado", 51, 75), new("Agudo", 76, 100)
        };

        var escalaCargaDeTrabalho = new List<DefinicaoLevel>
        {
            new("Muito Leve", 0, 20), new("Leve", 21, 40),
            new("Média", 41, 60), new("Alta", 61, 80),
            new("Muito Alta", 81, 100)
        };

        var escalaRelacionamento = new List<DefinicaoLevel>
        {
            new("Atenção", 1, 2.4), new("Zona de Alerta", 2.5, 3.4),
            new("Ambiente Saudável", 3.5, 5)
        };
        
        var escalas = new Dictionary<string, List<DefinicaoLevel>>
        {
            { "Mapeamento de Riscos - Ansiedade/Depressão/Burnout", escalaRiscos },
            
            { "Fatores de Carga de Trabalho", escalaCargaDeTrabalho },
            { "Sinais de Alerta", escalaCargaDeTrabalho }, 
            
            { "Diagnóstico de Clima - Relacionamento", escalaRelacionamento },
            { "Comunicação", escalaRelacionamento }, 
            { "Relação com a Liderança", escalaRelacionamento } 
        };
        return escalas;
    }
    public async Task<List<ResultadoCategoriaDto>> ResumoDasDezUltimasEntradasPorCategoriaAsync(string userId)
    {
        var respostas = await entradaDiariaRepository.ResumoDasDezUltimasEntradasPorCategoriaAsync(userId);

        if (!respostas.Any())
        {
            return new List<ResultadoCategoriaDto>();
        }
        
        var resultadosPorCategoria = new List<ResultadoCategoriaDto>();
        var agrupadoPorCategoria = respostas.GroupBy(resposta => resposta.Categoria);
        
        foreach (var grupoCategoria in agrupadoPorCategoria)
        {
            var categoriaAtual = grupoCategoria.Key;
            var respostasDaCategoria = grupoCategoria.ToList();
            var totalDeRespostasNaCategoria = respostasDaCategoria.Count;
            
            var respostaMaisFrequente = respostasDaCategoria
                .GroupBy(resposta => resposta.RespostaTexto)
                .OrderByDescending(grupo => grupo.Count())
                .First();
        
            var contagemMaisFrequente = respostaMaisFrequente.Count();
            
            var porcentagem = Math.Round((double)contagemMaisFrequente / totalDeRespostasNaCategoria * 100, 2);
            
            if (IniciarDefinicaoEscala().TryGetValue(categoriaAtual, out var definicoesDeNivel))
            {
                var nivel = definicoesDeNivel
                    .FirstOrDefault(l => porcentagem >= l.FaixaMin && porcentagem <= l.FaixaMax);

                if (nivel != null)
                {
                    resultadosPorCategoria.Add(new ResultadoCategoriaDto(categoriaAtual, nivel.Nivel));
                }
            }
        }
        
        return resultadosPorCategoria;
    }
}
