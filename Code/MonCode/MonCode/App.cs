using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Boutique.BDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace MonCode
{
    // Nom donné à un prototype de fonction
  



    internal static class App
    {
        // App.Services
        public static IServiceProvider Services { get; private set; }


        // Constructeur static 
        // Exécuté 1 fois unique avant tout accès à la classe
        static App()
        {
            var serviceCollection = new ServiceCollection();


            #region Configuration
            // design patter Builder
            // On dispose d'un builder => propose des méthodes pour configurer un objet
            // le builder possede une méthode qui créé l'objet
            var configBuilder = new ConfigurationBuilder();
            // J'ajoute à la config une source qui est un fichier xml
            configBuilder.AddXmlFile("App.config");
            IConfiguration config = configBuilder.Build();
            // Ajout de l'objet à l'injecteur de dépendance (il sera dispo partout sur demande de IConfiguration)
            serviceCollection.AddSingleton(config);
            #endregion

            #region Configuration BoutiqueContext
            // J'ajoute et je configure DI pour fournir un BoutiqueContextConfiguré
            serviceCollection.AddDbContext<BoutiqueContext>(options =>
            {
                // J'ajoute le package du provider pour la BDD qui m'intéresse
                // ici SQL Server
                // et j'utilise la méthode d'extension adaptée
                // ici => utiliser la config pour rechercher la chaine de connection nommée
                // MyConnection

                options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BoutiqueDB;Integrated Security=True;Trust Server Certificate=True;");
                //options.UseSqlServer(config.GetConnectionString("MyConnection"));
            });
            #endregion



            // Ajout et config des service de l'injection

            // Association dans les service entre une demande de ICatalogue
            // Avec la création d'une instance nouvelle de Catalogue

#if VERSIONLIMITEE
        serviceCollection.AddTransient<ICatalogue,Catalogue>(s=>new Catalogue(10));
#else
            serviceCollection.AddKeyedTransient<IProduit>("Fourchette", (s, o) => 
                new Produit("FOURCHETTE", 12,s));
            serviceCollection.AddKeyedTransient<IProduit>("Couteau", (s, o) => new Produit("COUTEAU", 23,s));
            // génération d'un produit aléatoire
            serviceCollection.AddKeyedTransient<IProduit>("Random", (s, o) => new Produit(new Regex(@"\-\d").Replace(Guid.NewGuid().ToString(), "", 1000).ToUpper(), new Random().Next(30, 1000),s));



            // La fonction (prototype StringValidator) qui permet de valider un Nom 
            serviceCollection.AddKeyedSingleton <StringValidator>("NomValidator", s =>
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    return (false, "La chaine ne peut être vide");
                }
                // utilisation de la config pour obtenir le pattern du Nom
                var pattern = config.GetRequiredSection("appSettings:patternNom").Value;
                var reg = new Regex(pattern);
                if (!reg.IsMatch(s))
                {
                    return (false, "La chaine doit contenir uniquement des lettres, chiffres, espace et tirets");
                }
                return (true, null);
            });

        
            serviceCollection.AddTransient<ICatalogue>(s => {
                ICatalogue c = new Catalogue(5);
                c.AddProduit(s.GetRequiredKeyedService<IProduit>("Fourchette"), 10);
                c.AddProduit(s.GetRequiredKeyedService<IProduit>("Couteau"), 10);
                c.AddProduit(new Produit("ASSIETTE", 76,App.Services));

                return c;
            });
#endif


            // Création de l'injecteur
            App.Services= serviceCollection.BuildServiceProvider();


            #region Assurer que la BDD est crée
            var db = App.Services.GetRequiredService<BoutiqueContext>();
            // Va créer la BDD avec la structure de tables nécessaire
            // si non existante
            // On peut aussi demander à mettre à jour la BDD si notre application a évolue
            db.Database.EnsureCreated();
            #endregion



        }
    }
}
