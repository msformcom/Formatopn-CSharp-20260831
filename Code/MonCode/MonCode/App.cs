using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace MonCode
{
    internal static class App
    {
        // App.Services
        public static IServiceProvider Services { get; private set; }


        // Constructeur static 
        // Exécuté 1 fois unique avant tout accès à la classe
        static App()
        {
            var serviceCollection = new ServiceCollection();
            // Ajout et config des service de l'injection

            // Association dans les service entre une demande de ICatalogue
            // Avec la création d'une instance nouvelle de Catalogue

#if VERSIONLIMITEE
        serviceCollection.AddTransient<ICatalogue,Catalogue>(s=>new Catalogue(10));
#else
            serviceCollection.AddKeyedTransient<IProduit>("Fourchette", (s, o) => new Produit("FOURCHETTE", 12));
            serviceCollection.AddKeyedTransient<IProduit>("Couteau", (s, o) => new Produit("COUTEAU", 23));

            // génération d'un produit aléatoire
            serviceCollection.AddKeyedTransient<IProduit>("Random", (s, o) => new Produit(new Regex(@"\-\d").Replace(Guid.NewGuid().ToString(), "", 1000).ToUpper(), new Random().Next(30, 1000)));

            serviceCollection.AddTransient<ICatalogue>(s => {
                ICatalogue c = new Catalogue(5);
                c.AddProduit(s.GetRequiredKeyedService<IProduit>("Fourchette"), 10);
                c.AddProduit(s.GetRequiredKeyedService<IProduit>("Couteau"), 10);
                c.AddProduit(new Produit("ASSIETTE", 76));

                return c;
            });
#endif


            // Création de l'injecteur
            App.Services= serviceCollection.BuildServiceProvider();
        }
    }
}
