using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using CantineServiceFromBDD;
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
            collection.AddTransient<ICantineService, CantineServiceBDD>();


            Services = collection.BuildServiceProvider();
        }
    }
}
