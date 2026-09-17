using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using CantineServiceFromAPI;
using HRDAL;
using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;

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

           

            #region Config de la config
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");
#if DEBUG      
                configBuilder.AddJsonFile("appsettings.dev.json");
         
#endif

            //configBuilder.AddXmlFile("app.config");

            // Je crée l'objet de type IConfiguration
            var config= configBuilder.Build();
            collection.AddSingleton < IConfiguration>(config);
            #endregion

            // Ajouter en singleton le DbDataModel qui provient de la config

            var dbDataModel=config.GetSection("metadata").Get<DbDataModel>()!;
            collection.AddSingleton<DbDataModel>(dbDataModel);


            // Chaque demande de ICantineService fera l'objet 
            // d'une nouvelle instanciation de CantineServiceAPI
            // Le type associé à ICantineService
            // Devra être défini dans un fichier de config

            // Le constructeur de CantineServiceAPI nécessite un HttpClient
            // AddHttpClient le fournit et gère le cycle de vie des connexions
            // L'API doit être démarrée pour que les tests passent
            collection.AddHttpClient<ICantineService, CantineServiceAPI>(client =>
            {
                client.BaseAddress = new Uri(config["ApiBaseUrl"]!);
            });

            // J'ajoute le CantineContext aux classes connues de ma collection
            collection.AddDbContext<CantineContext>(options =>
            {

                // Le OptionBuilder me permet de spécifier les options facilement
                // Par le biais de fonctions extensions définies dans le package du provider
                // TODO : Mettre la chaine de connection dans un fichier de config
                options.UseSqlServer("name=CantineDB")
                // Permet aux propriétés de navigation d'être
                // implémentées dans des classes heritières
                .UseLazyLoadingProxies();
                //options.UseSqlServer(config.GetConnectionString("CantineDB"));
            });
               

            collection.AddLogging(options =>
            {
                // Configuration de la journalisation
                // Importer un package spécialisé
                // Ajouter la config de journalisation via la méthode associé
                options.AddDebug();

            });


            collection.AddKeyedSingleton<Action<ModelBuilder>>("Builder1",modelBuilder =>
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


            Services = collection.BuildServiceProvider();

            // je demande un objet CantineContext Configuré
            var db = Services.GetRequiredService<CantineContext>();
            // EnsureCreated ne met pas à jour le schéma d'une BDD existante
            // et les tests consomment credit et stock : on repart d'une base vierge
            db.Database.EnsureDeleted();
            // Cette instruction créé la BDD si elle n'existe pas
            db.Database.EnsureCreated();



        }
    }
}
