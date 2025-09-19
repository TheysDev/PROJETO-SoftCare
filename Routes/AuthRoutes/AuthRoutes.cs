using SoftCare.Dtos;
using SoftCare.Services;

namespace SoftCare.Routes.AuthRoutes;

public static class AuthRoutes
{
    public static void MapAuthRoute(this WebApplication app)
    {
        var auth = app.MapGroup("/auth");
        
        auth.MapPost("/registrar", [AllowAnonymous] async (RegisterRequest request, IValidator<RegisterRequest> validator, IAuthService authService) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
            
            var result = await authService.RegistrarAsync(request);

            return !result.Sucesso ? Results.BadRequest(result) : Results.Created("/auth/login", new { message = "Usuário registrado com sucesso!" });
        });

        auth.MapPost("/login", [AllowAnonymous] async (LoginRequest request, IValidator<LoginRequest> validator, IAuthService authService) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await authService.LoginAsync(request);

            return !result.Sucesso ? Results.BadRequest(result) : Results.Ok(result);
        });
    }
}

