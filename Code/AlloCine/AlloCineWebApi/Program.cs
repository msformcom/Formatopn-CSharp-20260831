using System.Diagnostics;
using System.Reflection.Emit;
using AlloCineDAL;
using AlloCineInterfaces;
using AlloCineServiceBDD;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ajout des classes marquées avec l,'attribut [ApiController] à DI
builder.Services.AddControllers();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AlloCineContext>(builder =>
{
    // Mettre en place le provider
    // en spécifiant le nom de la chaine de connexion
    // enregistrée dans la config
    // var chaineDeConnection = config.GetConnectionString("AlloCineConnectionString");
    // builder.UseSqlServer(chaineDeConnection);
    builder.UseSqlServer("name=AlloCineConnectionString");
});


builder.Services.AddSingleton<Action<ModelBuilder>>(builder =>
{
    var f1 = new FilmDAO()
    {
        Code = "CO1981SC",
        Title = "La soupe aux choux",
        LastUpdate = DateTime.Now,
        Length = 96,
        ReleaseDate = new DateOnly(1081, 12, 02)
    };

    var c1 = new CinemaDAO()
    {
        Code = "CI422",
        Name = "Paradiso",
        OwnerName = "John Wick",
        LastUpdate = DateTime.Now,
        PostalCode = "75000",
        RoomCount = 1
    };

    var s1 = new SeanceDAO()
    {
        Code = "SE00001",
        IdCinema = c1.Id,
        IdFilm = f1.Id
    };

    builder.Entity<FilmDAO>(options =>
    {
        options.ToTable("TBL_Films");
        options.Property(c => c.Id).HasColumnName("PK_Film");
        options.Property(c => c.Title).IsUnicode();
        options.Property(c => c.Code).IsFixedLength().HasMaxLength(10).IsUnicode(false);


        options.HasData(f1);
    });
    builder.Entity<CinemaDAO>(options =>
    {
        options.ToTable("TBL_Cinemas");
        options.Property(c => c.Id).HasColumnName("PK_Cinema");
        options.Property(c => c.Name).IsUnicode();
        options.Property(c => c.Code).IsFixedLength().HasMaxLength(10).IsUnicode(false);
        options.HasData(c1);
    });
    builder.Entity<SeanceDAO>(options =>
    {
        options.ToTable("TBL_Seances");
        options.Property(c => c.Id).HasColumnName("PK_Seance");
        options.Property(c => c.IdCinema).HasColumnName("FK_Cinema");
        options.Property(c => c.IdFilm).HasColumnName("FK_Film");
        options.HasData(s1);
    });


});

builder.Services.AddTransient<IAlloCineService, AlloCineServiceFromDB>();




var app = builder.Build();

// J'intercale une fonction dans le pipeline des middlewar
app.Use(async (HttpContext context, Func<Task> next) =>
{
    // context => les informations sur la requète
    // next => exécute si je souhaite faire les traitement en aval du pipeline
    // Je journalise dans Debug l'adresse demandée

    Debug.WriteLine($"Début du traitement de {context.Request.Path}");
    var debut = DateTime.Now;

    // Lancer le traitement des Middleware qui suivent (en aval)
    await next();

    var ms = (DateTime.Now - debut).TotalMilliseconds;
    Debug.WriteLine($"Fin du traitement de {context.Request.Path} en {ms} ms");
});

// GET /count?start=1&end=5
//app.Use(async (HttpContext context, Func<Task> next) =>
//{
//    if (context.Request.Path == "/count")
//    {
//        var start = int.Parse(context.Request.Query["start"]!);
//        var end = int.Parse(context.Request.Query["end"]!);
//        await context.Response.WriteAsJsonAsync(Enumerable.Range(start, end).ToArray());
//    }
//    else
//    {
//        await next();
//    }
//});

// Ajoute les controllers en middleware
// Il faut que ce middleware puisse instancier via DI les controller
app.MapControllers();
// Middleware
//app.MapGet("/", () => "Hello World!");

app.Run();
