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

            // J'ajoute le CantineContext aux classes connues de ma collection
            collection.AddDbContext<CantineContext>(options =>
            {
                // Le OptionBuilder me permet de spécifier les options facilement
                // Par le biais de fonctions extensions définies dans le package du provider
                // TODO : Mettre la chaine de connection dans un fichier de config
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CantineDB;Integrated Security=True;Trust Server Certificate=true;");
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
                    options.Property(c => c.PublicId).IsRequired().HasMaxLength(5).IsFixedLength();

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
