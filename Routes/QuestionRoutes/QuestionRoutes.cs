using Microsoft.AspNetCore.Mvc;
using SoftCare.Services;

namespace SoftCare.Routes.QuestionRoutes;

public static class QuestionRoutes
{
    public static void MapQuestionRoutes(this WebApplication app)
    {
        var appRoutes = app.MapGroup("/app");

        appRoutes.MapGet("/question/{questionCode}", async (string questionCode, IQuestionService service) =>
        {
            var question = await service.GetQuestionDoFrontCode(questionCode);

            return !question.Sucesso ? Results.BadRequest("Questão não encontrada") : Results.Ok(question);
        });


        appRoutes.MapGet("/question/next",  async ([FromQuery] string category, [FromQuery] string? lastId, IQuestionService service) =>
        {
            if (string.IsNullOrEmpty(category))
            {
                return Results.BadRequest("O parâmetro 'category' é obrigatório.");
            }

            try
            {
                var nextQuestion = await service.GetNextQuestionDoFrontCategory(category, lastId);

                return nextQuestion == null ? Results.NoContent() : Results.Ok(nextQuestion);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        
        appRoutes.MapGet("/all-question/next",  async ([FromQuery] string? lastId, IQuestionService service) =>
        {
            try
            {
                var nextQuestion = await service.GetAllQuestionDoFront(lastId);

                return nextQuestion == null ? Results.NoContent() : Results.Ok(nextQuestion);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}