namespace MonCode;

using Boutique;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
[STATestClass]
public class CatalogueTests
{
    [TestMethod]
    [DataRow("100", false)]
    [DataRow("5", true)]


    public void AjoutProduitTest(string inputPrix, bool erreurExpected)
    {
        var c = App.Services.GetRequiredService<ICatalogue>();
        var p = App.Services.GetRequiredKeyedService<IProduit>("Random");
        Action action = () => c.AddProduit(new Produit("PRODUITTEST", decimal.Parse(inputPrix)));
        if (erreurExpected)
        {
            // Avec la valeur de inputPrix, on s'attend à ce que l'ajout du produit échoue et lève une exception
            Assert.ThrowsException<ArgumentException>(action, $"Une exception était attendue pour le prix");
        }
        else
        {
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


    [TestMethod]
    public void ProduitVenduTest()
    {
        // Arrange

        // Création du catalogue
        var c = App.Services.GetRequiredService<ICatalogue>();
        var p = App.Services.GetRequiredKeyedService<IProduit>("Random");


        // Vérification du stock du produit
        Assert.AreEqual(p.NbStock, 5, "Le stock initial du produit n'est pas correct");

        // gestion de l'eevénement ProduitVendu
        var evenementDeclenche = false;
        // Association de l'événement ProduitVendu à un gestionnaire d'événements


        EventHandler<ProduitVenduEventArgs> gestionnaire = (o, e) =>
        {
            evenementDeclenche = true;
            Assert.AreEqual(e.Quantite, 4,"Quantité vendue érronée dans le gestionnaire d'événements");
            Assert.AreEqual(e.Produit, p, "Produit vendu érroné dans le gestionnaire d'événements");
            Assert.AreEqual(e.MontantVente, 80, "Montant de la vente érroné dans le gestionnaire d'événements");
            // Ici, je code ce qui doit etre exécuté lorsque l'événement ProduitVendu est déclenché
            // Dans une Console : Console.WriteLine("Un produit a été vendu !");
        };

        c.ProduitVendu += gestionnaire;
        //c.ProduitVendu -= gestionnaire;



        // Act
        c.VendreProduit(p, 4);


        // Assert que l'événement ProduitVendu a été déclenché après la vente du produit
        Assert.IsTrue(evenementDeclenche, "L'événement ProduitVendu n'a pas été déclenché après la vente du produit.");
    }
    [TestMethod]
    public void ProduitEpuiseTest()
    {
        var c = App.Services.GetRequiredService<ICatalogue>();
        c.LimiteEpuisementProduit = 20;
        var p = new Produit("TEST", 20);
        c.AddProduit(p, 25);

        Boolean eventExecuted = false;
        c.ProduitPresqueEpuise += (o, e) =>
        {
            eventExecuted = true;
            throw new Exception();

        };


        c.VendreProduit(p, 2); // Stock 25 => 23
        Assert.IsFalse(eventExecuted, "L'évènement est exécuté sans épuisement");
        c.VendreProduit(p, 5); // Stock 23 => 18
        Assert.IsTrue(eventExecuted, "L'évènement est exécuté sans épuisement");

        eventExecuted = false;
        c.VendreProduit(p, 5); // Stock 18 => 13
        Assert.IsFalse(eventExecuted, "L'évènement est exécuté sans épuisement");

    }

   
}


