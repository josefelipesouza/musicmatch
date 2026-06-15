using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicMatch.Application.Commands.Artista;
using MusicMatch.Application.Interfaces;
using MusicMatch.Application.Validators;
using MusicMatch.Domain.Interfaces;
using MusicMatch.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using MusicMatch.Infrastructure.Messaging;
//using MusicMatch.Infrastructure.Geolocation;
using MusicMatch.Infrastructure.Persistence;
using MusicMatch.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(CadastrarArtistaCommand).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CadastrarArtistaValidator>();

// Entity Framework + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios
builder.Services.AddScoped<IArtistaRepository, ArtistaRepository>();
builder.Services.AddScoped<IContratanteRepository, ContratanteRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IMensagemRepository, MensagemRepository>();
builder.Services.AddScoped<IAgendaRepository, AgendaRepository>();


//builder.Services.AddHttpClient<IGeocodingService, GoogleGeocodingService>();

// RabbitMQ
builder.Services.AddSingleton<IMessageService>(sp =>
    new RabbitMqService(builder.Configuration["RabbitMQ:Host"] ?? "localhost"));
builder.Services.AddHostedService<RabbitMqConsumerService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://localhost:5174",
            "http://localhost:3000"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// JWT Service
builder.Services.AddSingleton<JwtService>();

// Google Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie("Cookies")
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});

// JWT Bearer
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Jwt:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Jwt:Secret"]!))
        };
    });

var app = builder.Build();

app.UseCors("AllowFrontend");

// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();