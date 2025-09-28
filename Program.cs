using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SoftCare.Data;
using SoftCare.Dtos;
using SoftCare.Repository;
using SoftCare.Routes.AuthRoutes;
using SoftCare.Routes.CheckInRoutes;
using SoftCare.Routes.QuestionRoutes;
using SoftCare.Services;
using SoftCare.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidator>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginValidator>();

builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp => 
{
    var settings = sp.GetRequiredService<IOptions<MongoDBConfig>>().Value;
    return new MongoClient(settings.ConnectionURI);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBConfig>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.NomeDatabase);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Key não localizada"))),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthRoute();
app.MapCheckInRoute();
app.MapQuestionRoutes();

app.Run();

