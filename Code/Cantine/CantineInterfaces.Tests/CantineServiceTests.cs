using CantineInterfaces.Tests.Models;
using HRDAL;
using HRDAL.DAO;
using Microsoft.Extensions.DependencyInjection;

namespace CantineInterfaces.Tests
{
    // Tests de comportement de l'implementation courante de ICantineService
    // Les methodes s'executent en parallele sur la meme BDD : tout test qui
    // modifie des donnees travaille sur un jeu qui lui est propre
    [TestClass]
    public sealed class CantineServiceTests
    {
        private static ICantineService Service(IServiceScope scope)
            => scope.ServiceProvider.GetRequiredService<ICantineService>();

        // Cree un employe et un article dedies au test appelant
        private static (string Matricule, string Reference) CreerJeuDeDonnees(
            CantineContext db, int stock, decimal prix, decimal credit)
        {
            // Reference et PublicId sont limites a 5 caracteres
            var suffixe = Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();

            var article = new ArticleDAO()
            {
                Reference = $"T{suffixe}",
                Label = $"Article {suffixe}",
                Price = prix,
                Stock = stock,
                Allergens = ""
            };
            var employe = new EmployeDAO()
            {
                PublicId = $"E{suffixe}",
                Name = "NOM",
                Surname = "PRENOM",
                BirthDate = new DateOnly(2000, 1, 1),
                CreditRepas = credit
            };

            db.Articles.Add(article);
            db.Employes.Add(employe);
            db.SaveChanges();

            return (employe.PublicId, article.Reference);
        }

        // Les jeux de donnees creees par les tests ne doivent pas rester
        // dans le catalogue une fois la serie terminee
        [ClassCleanup]
        public static void SupprimerLesJeuxDeDonnees()
        {
            using var scope = DI.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CantineContext>();

            var articles = db.Articles.Where(c => c.Reference.StartsWith("T")).ToList();
            var employes = db.Employes.Where(c => c.PublicId.StartsWith("E")).ToList();

            var idsArticles = articles.Select(a => a.Id).ToList();
            var idsEmployes = employes.Select(e => e.Id).ToList();

            // Les achats referencent article et employe : ils partent en premier
            db.Achats.RemoveRange(db.Achats.Where(
                a => idsArticles.Contains(a.IdArticle) || idsEmployes.Contains(a.IdEmploye)));
            db.SaveChanges();

            db.Articles.RemoveRange(articles);
            db.Employes.RemoveRange(employes);
            db.SaveChanges();
        }

        [TestMethod]
        public async Task ListeArticlesAsync_FiltreSurLeLibelle()
        {
            using var scope = DI.Services.CreateScope();

            var resultat = await Service(scope).ListeArticlesAsync(
                new ArticleSearch() { SearchText = "Steak", PrixMax = 1000 });

            var article = resultat.Single();
            Assert.AreEqual("S0001", article.Reference.Trim());
            Assert.AreEqual(15m, article.Prix);
        }

        [TestMethod]
        public async Task ListeArticlesAsync_FiltreSurLePrixMaximum()
        {
            using var scope = DI.Services.CreateScope();

            // Le steak est a 15 : seule la puree passe sous ce plafond
            var resultat = await Service(scope).ListeArticlesAsync(
                new ArticleSearch() { SearchText = "e", PrixMin = 12, PrixMax = 13 });

            Assert.IsTrue(resultat.All(c => c.Prix <= 13));
            Assert.IsTrue(resultat.Any(c => c.Reference.Trim() == "P0001"));
            Assert.IsFalse(resultat.Any(c => c.Reference.Trim() == "S0001"));
        }

        [TestMethod]
        public async Task ListeArticlesAsync_RenvoieLesAllergenes()
        {
            using var scope = DI.Services.CreateScope();

            var resultat = await Service(scope).ListeArticlesAsync(
                new ArticleSearch() { SearchText = "Steak", PrixMax = 1000 });

            // Le stockage est une chaine "Cianure, Gluten" decoupee sur la virgule
            var allergenes = resultat.Single().Allergenes.Select(a => a.Trim()).ToList();
            CollectionAssert.AreEquivalent(new[] { "Cianure", "Gluten" }, allergenes);
        }

        [TestMethod]
        public async Task LireEmployeInfosAsync_RenvoieLesInfosDuMatricule()
        {
            using var scope = DI.Services.CreateScope();

            var resultat = await Service(scope).LireEmployeInfosAsync("AA001");

            Assert.AreEqual("AA001", resultat.Matricule.Trim());
            Assert.AreEqual("NAME0001", resultat.Nom);
            Assert.AreEqual("SURNAME0001", resultat.Prenom);
            Assert.AreEqual(new DateOnly(1996, 2, 12), resultat.DateNaissance);
        }

        [TestMethod]
        public async Task ConsommerArticleAsync_DebiteLeCreditEtLeStock()
        {
            using var scope = DI.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CantineContext>();
            var (matricule, reference) = CreerJeuDeDonnees(db, stock: 5, prix: 10, credit: 100);

            await Service(scope).ConsommerArticleAsync(matricule, reference, 2);

            // La modification vient d'un autre contexte : on relit avec un scope neuf
            using var verif = DI.Services.CreateScope();
            var dbVerif = verif.ServiceProvider.GetRequiredService<CantineContext>();

            Assert.AreEqual(3, dbVerif.Articles.Single(c => c.Reference == reference).Stock);
            Assert.AreEqual(80m, dbVerif.Employes.Single(c => c.PublicId == matricule).CreditRepas);
            Assert.AreEqual(1, dbVerif.Achats.Count(a => a.Article.Reference == reference));
        }

        [TestMethod]
        public async Task ConsommerArticleAsync_ReferenceInconnue_Leve()
        {
            using var scope = DI.Services.CreateScope();

            var ex = await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => Service(scope).ConsommerArticleAsync("AA001", "XXXXX", 1));

            Assert.AreEqual("Pas d'article avec cette référence", ex.Message);
        }

        [TestMethod]
        public async Task ConsommerArticleAsync_MatriculeInconnu_Leve()
        {
            using var scope = DI.Services.CreateScope();

            var ex = await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => Service(scope).ConsommerArticleAsync("ZZZZZ", "P0001", 1));

            Assert.AreEqual("Pas d'employé avec ce matricule", ex.Message);
        }

        [TestMethod]
        public async Task ConsommerArticleAsync_StockInsuffisant_Leve()
        {
            using var scope = DI.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CantineContext>();
            var (matricule, reference) = CreerJeuDeDonnees(db, stock: 1, prix: 1, credit: 1000);

            var ex = await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => Service(scope).ConsommerArticleAsync(matricule, reference, 5));

            Assert.AreEqual("Stock insuffisant", ex.Message);
        }

        [TestMethod]
        public async Task ConsommerArticleAsync_CreditInsuffisant_Leve()
        {
            using var scope = DI.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CantineContext>();
            var (matricule, reference) = CreerJeuDeDonnees(db, stock: 100, prix: 50, credit: 10);

            var ex = await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => Service(scope).ConsommerArticleAsync(matricule, reference, 1));

            Assert.AreEqual("Crédit insuffisant", ex.Message);
        }

        [TestMethod]
        public async Task ConsommerArticleAsync_EchecNeModifieRien()
        {
            using var scope = DI.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CantineContext>();
            var (matricule, reference) = CreerJeuDeDonnees(db, stock: 2, prix: 10, credit: 100);

            await Assert.ThrowsExactlyAsync<ArgumentException>(
                () => Service(scope).ConsommerArticleAsync(matricule, reference, 3));

            using var verif = DI.Services.CreateScope();
            var dbVerif = verif.ServiceProvider.GetRequiredService<CantineContext>();

            Assert.AreEqual(2, dbVerif.Articles.Single(c => c.Reference == reference).Stock);
            Assert.AreEqual(100m, dbVerif.Employes.Single(c => c.PublicId == matricule).CreditRepas);
        }
    }
}
