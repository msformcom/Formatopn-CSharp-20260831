using System.Diagnostics;
using System.Reflection.Emit;
using CantineApi.CustomAttributes;
using CantineInterfaces;
using CantineServiceFromBDD;
using HRDAL;
using HRDAL.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);
// L'injecteur de dépendance est déjà présent dans ce système
// Avant de Builder l'application, je peux ajouter ici mes services
// La config est déjà associée a appsettings.json

// recherche toutes les classes de controller de mon assembly
// et les mets à disposition dans l'injecteur de dépendance
builder.Services.AddControllers(options =>
{
    // reponse personnalisée à un model non valid
    // options.Filters.Add<ValidateModelAttribute>();
});
// Add services to the container.

//
builder.Logging.AddDebug();

var dbDataModel = builder.Configuration.GetSection("metadata").Get<DbDataModel>()!;
builder.Services.AddSingleton<DbDataModel>(dbDataModel);


// Chaque demande de ICantineService fera l'objet 
// d'une nouvelle instanciation de CantineServiceBDD
// Le type associé à ICantineService
// Devra être défini dans un fichier de config

// Le constructeur de CantineServiceBDD nécessite la fourniture d'un CantineContext
builder.Services.AddTransient<ICantineService, CantineServiceBDD>();

// J'ajoute le CantineContext aux classes connues de ma collection
builder.Services.AddDbContext<CantineContext>(options =>
{

    // Le OptionBuilder me permet de spécifier les options facilement
    // Par le biais de fonctions extensions définies dans le package du provider
    // TODO : Mettre la chaine de connection dans un fichier de config
    options.UseSqlServer("name=CantineDB");
    // Permet aux propriétés de navigation d'être
    // implémentées dans des classes heritières
    //.UseLazyLoadingProxies();
    //options.UseSqlServer(config.GetConnectionString("CantineDB"));
});


//builder.Services.AddLogging(options =>
//{
//    // Configuration de la journalisation
//    // Importer un package spécialisé
//    // Ajouter la config de journalisation via la méthode associé
//    options.AddDebug();

//});


builder.Services.AddKeyedSingleton<Action<ModelBuilder>>("Builder1", modelBuilder =>
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







var app = builder.Build(); // Créer un serveur Web


// Log
app.Use(async (HttpContext context, Func<Task> next) =>
{
    // context = Objet me permettant d'avoir le contexte de la requete
    // url, body, headers, cookies..
    // Response => méthodes pour répondre
    // next => Fonction a exécuter pour passer la main aux middleware suivants
    var logger = app.Services.GetRequiredService<ILogger<ApplicationBase>>();
    logger.LogInformation($"Entrée de la requète {context.Request.Path}");
    var watch = Stopwatch.StartNew();
    // Passage de la requète aux middleware suivants
    await next();
    if (context.Response.StatusCode != 200)
    {
        if (context.Items.ContainsKey("Erreur"))
        {
            var ex = (Exception)context.Items["Erreur"];
            logger.LogError(ex.Message);
        }

    }
    logger.LogInformation($"Sortie de la requète {context.Request.Path} en {watch.ElapsedMilliseconds}");
});
// Gestion de l'erreur
app.Use(async (HttpContext context, Func<Task> next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Items["Erreur"] = ex;
        context.Response.StatusCode = 500;
        //context.Response.WriteAsync()

    }

});

// C'est une méthode qui recherce des controller 
// cad Classe qui contient des méthodes à mettre à disposition des requetes
app.MapControllers();

app.MapGet("/Addition/{a:int}/{b:int}", (int a, int b, [FromServices] IConfiguration config) =>
{
    throw new Exception("Erreur volontaire");
    return a + b;
});




app.Run(); // Démarrer
