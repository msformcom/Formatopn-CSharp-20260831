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
            var db = DI.Services.GetRequiredService<CantineContext>();

            // J'obtiens les employes avec les achats et les articles
            // eager Loading=> Chargement des achats le plus tot possioble
            var employes = db.Employes
                .Include(c=>c.Achats)
                    .ThenInclude(a=>a.Article)
                    .Where(c => c.CreditRepas>0).ToList();

            // La liste des achats est présente car Include(c=>c.Achats) => Jointure
            var employe = employes.FirstOrDefault();
            
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
