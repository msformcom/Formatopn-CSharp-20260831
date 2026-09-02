namespace MonCode;

using Boutique;

[TestClass]
public class CatalogueTests
{
    [TestMethod]
    [DataRow("100", false)]
    [DataRow("5", true)]
    public void AjoutProduitTest(string inputPrix, bool erreurExpected)
    {
        var c = new Catalogue(10);
        Action action=() => c.AddProduit(new Produit("PRODUITTEST", decimal.Parse(inputPrix)));
        if(erreurExpected)
        {
            // Avec la valeur de inputPrix, on s'attend à ce que l'ajout du produit échoue et lève une exception
            Assert.ThrowsException<Exception>(action, $"Une exception était attendue pour le prix");
        }
        else {
            // Avec la valeur de inputPrix, on s'attend à ce que l'ajout du produit réussisse
            action();
            Assert.IsTrue(c.ListeProduits.Any(p => p.Nom == "PRODUITTEST" && p.Prix == decimal.Parse(inputPrix)), $"Le produit n'a pas été ajouté correctement");
        }

        var ListeProduits = c.ListeProduits.Where(p => p.Prix <= 100)
                .OrderByDescending(c => c.Prix)
                .ThenBy(c => c.Nom).ToList(); // Liste des produits dont le prix est inférieur à 100 ordonés par prix decroissant
        var ListeProduits2 = c.ListeProduits.Where(p => p.NbStock > 1 && p.Nom.Contains("TELE"))
                        .OrderBy(c => c.Prix).Take(3).ToList(); // Liste des produits dont le nom contient "TELE" et le nbStock>1 (juste les 3 modeles les moins chers)
        //ListeProduits2.First().Prix = 12;
        //ListeProduits2.First().NbStock = 0;
    }

}
