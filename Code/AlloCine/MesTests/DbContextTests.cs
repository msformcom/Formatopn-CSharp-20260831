using AlloCineDAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeCoverage.Core;
using Microsoft.Extensions.Logging;

namespace MesTests
{
    [TestClass]
    public sealed class DbContextTests
    {
        // Services me permet de construire des instances de class à partir des interface

        IServiceProvider Services = null;

        public DbContextTests()
        {
            // Configurer l'injection de dépendance

            #region Configuration
            // Création et ajout de la config à l'injecteur
            // désign patter Builder
            // construire un objet en utilisant un objet spécialisé dans la construction

            var configBuilder = new ConfigurationBuilder();
            // J'ai ajouté le package Microsoft.Extensions.Configuration.Json
            // La méthode AddJsonFile va lire le fichier json

            configBuilder.AddJsonFile("appsettings.json");

            // Construction de l'instance de IConfiguration par le builder
            var config = configBuilder.Build();
            #endregion





            #region Configuration et construction de l'injecteur DI
            // Builder de IServiceProvider
            var serviceCollection= new ServiceCollection();
            // En cas de demande de IConfiguration => config renvoyé
            serviceCollection.AddSingleton<IConfiguration>(config);


            #region logging
            serviceCollection.AddLogging(builder =>
            {
                // Installation du package Microsoft.Extensions.Logging.Debug
                builder.AddDebug();
            });
            #endregion


            #region Configuration des options pour AlloCineContext
            // AddDbContext ajoute le AlloXCineContext aux services
            // Me permet d'utiliser un builder pour générer l'objet OptionBuilder
            serviceCollection.AddDbContext<AlloCineContext>(builder =>
            {
                // Mettre en place le provider
                // en spécifiant le nom de la chaine de connexion
                // enregistrée dans la config
                // var chaineDeConnection = config.GetConnectionString("AlloCineConnectionString");
                // builder.UseSqlServer(chaineDeConnection);
                builder.UseSqlServer("name=AlloCineConnectionString");
            });

            // Fonction qui termine la construction de la BDD
            serviceCollection.AddSingleton<Action<ModelBuilder>>(builder =>
            {
                var f1 = new FilmDAO()
                {
                    Code = "CO1981SC",
                    Title = "Las soupe aux choux",
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
                    Code="SE00001",
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


            #endregion

            // Une fois la collection remplie avec les services nécessaires à mon appli
            // Je build le provider et le mets à disposition
            this.Services= serviceCollection.BuildServiceProvider();

            #endregion


        }


        [TestMethod]
        public void CreateDatabaseTest()
        {
            // Je demande à l'injecteur de dépendance de construire une instance de AlloCineContext
            // il va se charger de construire les objets nécessaires (dans le construteur)
            var db =this.Services.GetService<AlloCineContext>();

            if(db.Database.EnsureCreated())
            {
                var logger = this.Services.GetService<ILogger<DbContextTests>>();
                logger.LogInformation("La BDD vient d'être créée");
            }

           


        }
    }
}
