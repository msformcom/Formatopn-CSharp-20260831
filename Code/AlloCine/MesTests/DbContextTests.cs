using AlloCineDAL;
using Microsoft.Extensions.DependencyInjection;
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
            this.Services = DI.GetInjector();


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
