using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MonCode;

[TestClass]
public class ClasseTests
{

    [TestMethod]
    [DataRow("10", "20", false)]
    [DataRow("10", "-20", true)]
    public void ValidationChangementPrix(string prixInitialString, string nouveauPrixString, bool erreur)
    {
        fjlkmddecimal prixInitial = decimal.Parse(prixInitialString);
        decimal nouveauPrix = decimal.Parse(nouveauPrixString);
        // Arrange 
        bool exception = false;
        var p = new Produit(prixInitial);

        try
        {
            p.Prix = nouveauPrix;
        }
        catch (Exception)
        {
            exception = true;
        }
        // Act


        // Assert
        Assert.AreEqual(erreur, exception, "Le prix ne doit pas passer si non valide");
        if (exception == false)
        {
            Assert.AreEqual(nouveauPrix, p.Prix, "Le prix n'a pas été modifié correctement");
        }

    }

    [TestMethod]
    public void ValidationTest()
    {
        // Arrange
        Produit produit = new Produit(1000);
        bool exceptionPrixNegatif = false;
        // Act : une instance doit avoir un prix par défaut supérieur à 0
        Assert.IsTrue(produit.Prix > 0, "La valeur par défaut ne convient pas");

        try
        {
            produit.Prix = -10;
        }
        catch (Exception)
        {
            exceptionPrixNegatif = true;
        }

        // Assert : Vérifier le résultat du Act
        Assert.IsTrue(exceptionPrixNegatif, "Le prix ne doit pas passer si négatif");



    }
}
