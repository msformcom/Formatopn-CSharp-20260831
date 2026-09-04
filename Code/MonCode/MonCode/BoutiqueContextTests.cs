using System;
using System.Collections.Generic;
using System.Text;
using Boutique.BDD;
using Microsoft.Extensions.DependencyInjection;


namespace MonCode
{
    [TestClass]
    public class BoutiqueContextTests
    {
        [TestMethod]
        public void InsertProduitInBDDTest()
        {
            // Produit à inserer
            var p = new ProduitBDD()
            {
                IdCatalogue = Guid.NewGuid(),
                Nom = "Fourchette",
                NbStock = 2,
                Prix = 25
            };
            // Obtention du contexte
            BoutiqueContext db = App.Services.GetRequiredService<BoutiqueContext>();
            // Ajout du produit au DbSet (comme une liste)

            db.Produits.Add(p);




            // MAJ dans la BDD
            db.SaveChanges();



        }




    }
}
