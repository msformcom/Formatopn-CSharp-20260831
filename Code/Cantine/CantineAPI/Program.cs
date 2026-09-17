using System.Diagnostics;
using CantineInterfaces;
using CantineServiceFromBDD;
using HRDAL;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Les metadonnees de la BDD proviennent de la config, comme dans le projet de tests
var dbDataModel = builder.Configuration.GetSection("metadata").Get<DbDataModel>()!;
builder.Services.AddSingleton<DbDataModel>(dbDataModel);

builder.Services.AddTransient<ICantineService, CantineServiceBDD>();

builder.Services.AddDbContext<CantineContext>(options =>
{
    options.UseSqlServer("name=CantineDB");
});

// Nommage des tables, propre a cette application
// Les regles sont partagees avec le projet de tests via CantineTables
builder.Services.AddKeyedSingleton<Action<ModelBuilder>>("Builder1",
    (_, _) => CantineTables.Configure);

var app = builder.Build();

// Cree la BDD si elle n'existe pas encore, sans jamais la supprimer
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<CantineContext>().Database.EnsureCreated();
}

// Configure the HTTP request pipeline.

// Journalisation de chaque requête : méthode, chemin, code de retour, durée
// Placé en premier pour englober tout le reste du pipeline
app.Use(async (context, next) =>
{
    var chrono = Stopwatch.StartNew();

    // Laisse passer la requête vers la suite du pipeline
    await next();

    // Au retour, la réponse est connue : on peut lire son code
    app.Logger.LogInformation("{Methode} {Chemin} => {Code} en {Duree}ms",
        context.Request.Method,
        context.Request.Path,
        context.Response.StatusCode,
        chrono.ElapsedMilliseconds);
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Interface de test de l'API, alimentée par le document OpenAPI
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
