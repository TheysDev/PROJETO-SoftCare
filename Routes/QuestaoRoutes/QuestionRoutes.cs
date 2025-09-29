using Microsoft.AspNetCore.Mvc;
using SoftCare.Services;

namespace SoftCare.Routes.QuestionRoutes;

public static class QuestionRoutes
{
    public static void MapQuestionRoutes(this WebApplication app)
    {
        var appRoutes = app.MapGroup("/app");

        appRoutes.MapGet("/question/{questionCode}", async (string questionCode, IQuestaoService service) =>
        {
            var question = await service.BuscarQuestaoPeloCodeAsync(questionCode);

            return !question.Sucesso ? Results.BadRequest("Questão não encontrada") : Results.Ok(question);
        });


        appRoutes.MapGet("/question/next",  async ([FromQuery] string category, [FromQuery] string? lastId, IQuestaoService service) =>
        {
            if (string.IsNullOrEmpty(category))
            {
                return Results.BadRequest("O parâmetro 'category' é obrigatório.");
            }

            try
            {
                var nextQuestion = await service.PagBuscarQuestaoPelaCategoryAsync(category, lastId);

                return nextQuestion == null ? Results.NoContent() : Results.Ok(nextQuestion);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        
        appRoutes.MapGet("/all-question/next",  async ([FromQuery] string? lastId, IQuestaoService service) =>
        {
            try
            {
                var nextQuestion = await service.BuscarTodasAsQuestoesAsync(lastId);

                return nextQuestion == null ? Results.NoContent() : Results.Ok(nextQuestion);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}