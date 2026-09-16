using CantineInterfaces.Tests.Models;
using CantineServiceFromBDD.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CantineInterfaces.Tests
{
    [TestClass]
    public sealed class ICantineServiceTests
    {
        [TestMethod]
        public async Task ListeArticlesAsyncTests()
        {
            // Arrange
            // Avoir l'instance de ICantineService à tester
            // Ici on ne sait pas quel est le type précis que l'on va tester
            // Le CantineContext est Scoped : un scope par test évite de le partager
            // entre les méthodes exécutées en parallèle
            using var scope = DI.Services.CreateScope();
            ICantineService instance = scope.ServiceProvider.GetService<ICantineService>();
            Assert.IsNotNull(instance);

            var search = new ArticleSearch() { SearchText = "u", PrixMax = 1000 };

            var resultat = await instance.ListeArticlesAsync(search);

            // declarativement resultat est un IEnumerable
            // mais son contenu peut être un IQueryable
            IEnumerable<IArticle> resultat2;
            if(resultat is IQueryable<IArticle> query) {
                resultat2 = query.Where(c => c.Libelle.Length > 10).ToList();
            }
            else
            {
                resultat2 = resultat.Where(c => c.Libelle.Length > 10).ToList();
            }



            // Le AfterMap du mapping découpe la chaine Allergens de la BDD
            var avecAllergenes = resultat.First(a => a.Allergenes.Count >= 2);
            Assert.AreEqual(2, avecAllergenes.Allergenes.Count);

        }

        [TestMethod]
        public async Task MyTestMethod()
        {
            using var scope = DI.Services.CreateScope();
            ICantineService instance = scope.ServiceProvider.GetRequiredService<ICantineService>();
            var resultat=await instance.LireEmployeInfosAsync("AA001");

            Assert.IsNotNull(resultat);


        }
    }
}
