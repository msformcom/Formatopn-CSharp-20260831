using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AlloCineDAL;
using AlloCineInterfaces;
using AlloCineServiceApi;
using AlloCineServiceBDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MesTests
{
    internal class DI
    {
        public static IServiceProvider GetInjector()
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
            var serviceCollection = new ServiceCollection();
            // En cas de demande de IConfiguration => config renvoyé
            serviceCollection.AddSingleton<IConfiguration>(config);


            #region logging
            serviceCollection.AddLogging(builder =>
            {
                // Installation du package Microsoft.Extensions.Logging.Debug
                builder.AddDebug();
            });
            #endregion

            switch (config.GetSection("TypeTest").Value)
            {
                case "API":
                    serviceCollection.AddTransient<HttpClient>(
                        s => new HttpClient() { BaseAddress = new Uri(config.GetSection("ApiUrl").Value) });
                    serviceCollection.AddTransient<IAlloCineService, AlloCineServiceFromApi>();

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
                        var cat1 = new CategorieDAO() { Code = "Cat1", Label = "Comédie" };
                        var cat2 = new CategorieDAO() { Code = "Cat2", Label = "Horreur" };
                        var f1 = new FilmDAO()
                        {
                            Code = "CO1981SC",
                            Title = "La soupe aux choux",
                            IdCategorie=cat1.Id,
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

                        builder.Entity<CategorieDAO>(options =>
                        {
                            options.ToTable("TBL_Categories");
                            options.Property(c => c.Id).HasColumnName("PK_Categorie");
                            options.Property(c => c.Label).IsUnicode();
                            options.Property(c => c.Code).IsFixedLength().HasMaxLength(10).IsUnicode(false);
                            options.HasData(cat1,cat2);
                        });

                        builder.Entity<FilmDAO>(options =>
                        {
                            options.ToTable("TBL_Films");
                            options.Property(c => c.Id).HasColumnName("PK_Film");
                            options.Property(c => c.IdCategorie).HasColumnName("FK_Categorie");
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
                    //serviceCollection.AddTransient<IAlloCineService, AlloCineServiceFromDB>();
                    break;
                
                default:
                    break;
            }



            // Ajoute le service IAlloCineService
            // Et je le configure pour que ce soit la class AlloCineServiceFromDB
            // qui soit fournie

            var typeServiceString = "AlloCineServiceBDD.AlloCineServiceFromDB";
            var typeService = Assembly.GetExecutingAssembly().GetType(typeServiceString!);
          

            //var typeServiceString = config.GetRequiredSection("typeServiceString").Value;
            //var assembly = Assembly.LoadFrom(@"c:\biblio\AlloCineServiceBDD.dll");
            
            
      



     
            
 

          
            //serviceCollection.AddTransient(typeof(IAlloCineService),typeService);

            // Une fois la collection remplie avec les services nécessaires à mon appli
            // Je build le provider et le mets à disposition
            return serviceCollection.BuildServiceProvider();

            #endregion
        }
    }
}
