using System;
using System.Collections.Generic;
using System.Text;
using HRDAL;
using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CantineInterfaces.Tests
{
    [TestClass]
    public  class CantineContextTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            using var scope = DI.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CantineContext>();

            // Ce test a besoin d'au moins un achat pour démontrer les chargements
            // Le créer ici le rend indépendant de l'ordre d'exécution des autres tests
            var employeSeed = db.Employes.First(c => c.PublicId == "AA002");
            var articleSeed = db.Articles.First(c => c.Reference == "S0001");
            db.Achats.Add(new AchatDAO() { IdEmploye = employeSeed.Id, IdArticle = articleSeed.Id, Quantite = 1 });
            db.SaveChanges();

            // J'obtiens les employes avec les achats et les articles
            // eager Loading=> Chargement des achats le plus tot possioble
            var employes = db.Employes
                .Include(c=>c.Achats)
                    .ThenInclude(a=>a.Article)
                    .Where(c => c.CreditRepas>0).ToList();

            // La liste des achats est présente car Include(c=>c.Achats) => Jointure
            var employe = employes.First(c => c.Achats.Any());

            var achats=employe.Achats; // Hashset Vide car non chargé si pas Include
            db.Entry(employe).Collection(c => c.Achats).Load(); // Chargement Explicite

            // Article non présent 
            var achat = achats.First();


            var article = achat.Article; // null
            // Explicit loading => Sur un achat (sans article chargé)
            // Je demande le chargement de l'article
            // SELECT ... FROM TBL_Article WHERE PK_Article=...
            db.Entry(achat).Reference(c => c.Article).Load();

            // Chargement par Proxy 
            //class EmployeDAO
            //{
            //    public virtual ICollection<AchatDAO> Achats { get; set; } = new HashSet<AchatDAO>();
            //}
            // On peut hériter de EmployeDAO et réimplementer le get de Achats
            // => Implementer le SELECT pour obtenir la liste des achats



    }
}
}
