using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using CantineServiceFromBDD;
using HRDAL;
using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CantineInterfaces.Tests
{
    public static class DI
    {
        // Accès par DI.Services
        public static IServiceProvider Services { get; private set; }

        // Constructeur static
        // Exécuté une seule fois 
        static DI()
        {
            // Design patter builer => objet qui sert à construire un autre objet
            var collection = new ServiceCollection();

            // Chaque demande de ICantineService fera l'objet 
            // d'une nouvelle instanciation de CantineServiceBDD
            // Le type associé à ICantineService
            // Devra être défini dans un fichier de config

            // Le constructeur de CantineServiceBDD nécessite la fourniture d'un CantineContext
            collection.AddTransient<ICantineService, CantineServiceBDD>();

            // Fournit un ILoggerFactory à toute la collection
            // EF Core s'en sert pour tracer les requêtes SQL générées
            collection.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Les règles de conversion sont déclarées en static dans CantineMapping
            collection.AddAutoMapper(CantineMapping.Configure);

            // J'ajoute le CantineContext aux classes connues de ma collection
            collection.AddDbContext<CantineContext>(options =>
            {
                // Le OptionBuilder me permet de spécifier les options facilement
                // Par le biais de fonctions extensions définies dans le package du provider
                // TODO : Mettre la chaine de connection dans un fichier de config
                options.UseSqlServer("Server=127.0.0.1,1433;Initial Catalog=CantineDB;User Id=sa;Password=Librodocus!2026;Trust Server Certificate=true;");

                // Affiche la valeur des paramètres dans les logs SQL
                // Réservé aux tests : expose les données en clair
                options.EnableSensitiveDataLogging();
            });


            collection.AddKeyedSingleton<Action<ModelBuilder>>("Builder1",modelBuilder =>
            {
                modelBuilder.Entity<ArticleDAO>(options =>
                {
                    options.ToTable("TBL_Articles");
                    options.Property(c => c.Id).HasColumnName("PK_Article");
                    options.Property(c => c.Reference).IsRequired().HasMaxLength(5).IsFixedLength();

                });

                modelBuilder.Entity<EmployeDAO>(options =>
                {
                    options.ToTable("TBL_Employes");
                    options.Property(c => c.Id).HasColumnName("PK_Employe");
                    options.Property(c => c.PublicId).IsRequired().HasMaxLength(6).IsFixedLength();

                });
            });


            Services = collection.BuildServiceProvider();

            // je demande un objet CantineContext Configuré
            var db = Services.GetRequiredService<CantineContext>();
            // Cette instruction créé la BDD si elle n'existe pas
            db.Database.EnsureCreated();



        }
    }
}
