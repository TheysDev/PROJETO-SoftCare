using System.Security.Claims;
using SoftCare.Dtos.Respostas;
using SoftCare.Services;
using SoftCare.Validations;

namespace SoftCare.Routes.CheckInRoutes;

public static class CheckInRoute
{
    public static void MapEntradaDiariaRoute(this WebApplication app)
    {
        var appRoutes = app.MapGroup("/app");

        appRoutes.MapPost("/entrada-diaria",
            async (ClaimsPrincipal user, IValidator<RespostasRequest> validator, List<RespostasRequest> request,
                IEntradaDiariaService service) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    Results.Unauthorized();
                }

                foreach (var req in request)
                { 
                    var validationResult = await validator.ValidateAsync(req);
                    
                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                }
                
                if (request == null || request.Count == 0)
                {
                    return Results.BadRequest("A lista de respostas não pode ser vazia.");
                }

                var result = await service.RegistrarEntradaDiariaAsync(request, userId);

                return Results.Created($"/daily-entries/{result.Dado}", result);
            });

        appRoutes.MapGet("/entrada-diaria/resumo", async (ClaimsPrincipal user, IEntradaDiariaService service) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Results.Unauthorized();
            }

            var result = await service.ResumoDasDezUltimasEntradasDoUsuarioAsync(userId);

            if (result == null || !result.Any())
            {
                return Results.NotFound("Nenhuma análise encontrada para as últimas 10 entradas deste usuário.");
            }

            return Results.Ok(result);
        });
        
        appRoutes.MapGet("/categoria/resumo", async (ClaimsPrincipal user, IEntradaDiariaService service) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var resultado = await service.ResumoDasDezUltimasEntradasPorCategoriaAsync(userId);
            
            return Results.Ok(resultado);
        });
    }
}