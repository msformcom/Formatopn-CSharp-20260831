using System.Diagnostics;
using CantineInterfaces;
using CantineServiceFromBDD;
using HRDAL;
using HRDAL.DAO;
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
builder.Services.AddKeyedSingleton<Action<ModelBuilder>>("Builder1", (_, _) => (ModelBuilder modelBuilder) =>
{
    modelBuilder.Entity<ArticleDAO>(options =>
    {
        options.ToTable("TBL_Articles");
        options.Property(c => c.Id).HasColumnName("PK_Article");
        options.Property(c => c.Reference).IsRequired().HasMaxLength(5).IsFixedLength();
    });

    modelBuilder.Entity<AchatDAO>(options =>
    {
        options.ToTable("TBL_Achats");
        options.Property(c => c.Id).HasColumnName("PK_Achat");
        options.Property(c => c.IdEmploye).HasColumnName("FK_Employe");
        options.Property(c => c.IdArticle).HasColumnName("FK_Article");
    });

    modelBuilder.Entity<EmployeDAO>(options =>
    {
        options.ToTable("TBL_Employes");
        options.Property(c => c.Id).HasColumnName("PK_Employe");
        options.Property(c => c.PublicId).IsRequired().HasMaxLength(5).IsFixedLength();
    });
});

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
