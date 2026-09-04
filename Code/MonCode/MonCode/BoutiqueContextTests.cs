using System;
using System.Collections.Generic;
using System.Text;
using Boutique.BDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace MonCode
{
    [TestClass]
    public class BoutiqueContextTests
    {


        [TestMethod]
        // S SEARCH     => SELECT Id,Nom,Prenom FROM Employes WHERE IdService=2
        // C CREATE     => INSERT INTO Employes...
        // R READ       => SELECT * FROM EMployes WHERE Id=678
        // U UPDATE     => UPDATE...
        // D DELETE     => DELETE FROM 
        public void SCRUDProduitInBDDTest()
        {
            //INSERTION
            var p = new ProduitBDD()
            {
                IdCatalogue = Guid.NewGuid(),
                Nom = "FOURCHETTE",
                NbStock = 2,
                Prix = 25
            };
            // Obtention du contexte
            BoutiqueContext db = App.Services.GetRequiredService<BoutiqueContext>();
            // Ajout du produit au DbSet (comme une liste)

            db.Produits.Add(p);




            // MAJ dans la BDD
            db.SaveChanges();


            // Update
            db = App.Services.GetRequiredService<BoutiqueContext>();
            // Recherche du produit
            var pAMAJ=db.Produits.FirstOrDefault(c => c.Nom == "FOURCHETTE");
            if (pAMAJ == null)
            {
                throw new KeyNotFoundException("Le produit n'existe pas");
            }
            pAMAJ.NbStock += 10;
            db.SaveChanges();

            // Select
            db = App.Services.GetRequiredService<BoutiqueContext>();
            // R
            // SELECT Nom AS Name,Prix AS Price
            // FROM TBL_Employes
            // WHERE NbStock>10
            // ORDER BY Nom ASC
            var produits = db.Produits
                            .Where(c => c.NbStock > 0)
                            .OrderBy(c => c.Nom)
                            .Select(c=> new {Name=c.Nom,Price=c.Prix});

            var produitText = produits.ToQueryString();
            // produits : IEnumerable => La requete n'est pas utilisée tant que
            // on ne lit pas les résultats
            // produits : IQueryable

            // SELECT Nom AS Name,Prix AS Price
            // FROM TBL_Employes
            // WHERE NbStock>10
            // GROUP BY Prix
            // ORDER BY Nom ASC
            var produitsRegroupes = produits.GroupBy(c => c.Price);

  

            var autreSelection=produitsRegroupes.Select(c=>new  {Prix=c.Key, Nombre=c.Count() });
            var autreSelectionText = autreSelection.ToQueryString();

            // SELECT * FROM TBL_Produits WHERE Nom LIKE 'F%' AND ?
            // COSH n'existe pas 
            var selection = db.Produits.Where(c => c.Nom.StartsWith("F") && Math.Acosh(c.NbStock) > 0);

            
            var selection2 =
                            // Partie de la requète gérée par le serveur de BDD
                            db.Produits.Where(c => c.Nom.StartsWith("F"));
            var queryText2 = selection2.ToQueryString();
            var selection3=selection2
                            // As Enumerable termine le IQueryable
                            .AsEnumerable()
                            // Partie de la selection qui est gérée par les objets en mémoire
                            .Where(c => Math.Acosh(c.NbStock) > 0)
                            .Take(2);

            // Flux
            // Lecture 100 enregistrement disque server
            // => envoi sur réseau 100
            // Désérialisation en classe
            // Filtre Acosh
            // Take 2 => Pas Stop
            // Lecture 100 enregistrement disque server
            // => envoi sur réseau 100
            // Désérialisation en classe
            // Filtre Acosh
            // Take 2 => Stop



            var produitListe = selection2.ToList();





        }




    }
}
