using CantineInterfaces.Tests.Models;
using CantineServiceFromBDD.Models;
using HRDAL;
using Microsoft.EntityFrameworkCore;
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

            var search = new ArticleSearch() { SearchText = "a", PrixMax = 1000 };

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

        [TestMethod]
        public async Task ConsommerArticleAsyncTests()
        {
            using var scope = DI.Services.CreateScope();
            ICantineService instance = scope.ServiceProvider.GetRequiredService<ICantineService>();

            // Steak à 15, deux parts pour AA001 => 30 débités sur les 100 de crédit
            var achat = await instance.ConsommerArticleAsync("AA001", "S0001", 2);

            Assert.AreEqual("AA001", achat.MatriculeEmploye);
            Assert.AreEqual("S0001", achat.ReferenceArticle);
            Assert.AreEqual(2, achat.Quantite);
            Assert.AreEqual(30m, achat.Prix);

            // Purée à 12, une part pour AA002
            await instance.ConsommerArticleAsync("AA002", "P0001");

            // La ligne est bien écrite en base et le crédit suivi
            var db = scope.ServiceProvider.GetRequiredService<CantineContext>();
            var employe = await db.Employes.FirstAsync(c => c.PublicId == "AA001");

            Assert.AreEqual(70m, employe.CreditRepas);
            Assert.AreEqual(1, await db.Achats.CountAsync(c => c.IdEmploye == employe.Id));
        }

        [TestMethod]
        public async Task ConsommerArticleAsyncRefusTests()
        {
            using var scope = DI.Services.CreateScope();
            ICantineService instance = scope.ServiceProvider.GetRequiredService<ICantineService>();

            // Référence inconnue
            await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => instance.ConsommerArticleAsync("AA001", "ZZZZZ"));

            // Matricule inconnu
            await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => instance.ConsommerArticleAsync("ZZ999", "S0001"));

            // Steak à 15 : 10 parts dépassent les 100 de crédit
            await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => instance.ConsommerArticleAsync("AA002", "S0001", 10));
        }
    }
}
