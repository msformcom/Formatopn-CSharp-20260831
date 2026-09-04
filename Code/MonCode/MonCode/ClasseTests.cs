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
        decimal prixInitial = decimal.Parse(prixInitialString);
        decimal nouveauPrix = decimal.Parse(nouveauPrixString);
        // Arrange 
        bool exception = false;
        var p = new Produit("TOTO",prixInitial,App.Services);

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
    public void ValidationNom()
    {
        // Arrange
        Produit p = null;
        Boolean exception = false;

        // Act
        try
        {
            // Sensé renvoyer une exception
             p = new Produit("Toto", 1000, App.Services);
           
        }
        catch (Exception)
        {
            exception = true;
 
        }
        Assert.IsTrue(exception, "Le nom du produit incorrect passe");

        // Vérifie qu'une fonction f déclenche une Exeption
        T ThrowsException<T>(Action f , string message) where T:Exception
        {
            try
            {
                f();
                Assert.Fail(message);
                return null;
            }
            catch (Exception ex)
            {
                if(!(ex is T))
                {
                    Assert.Fail(message);
                }
                return (T)ex;
            }
         
        }


        var ex=Assert.ThrowsException<ArgumentException>(() =>
        {
            // Sensé renvoyer une exception
            p = new Produit("Toto", 1000,App.Services);
        },"Le nom du produit incorrect passe");
        Assert.AreEqual(ex.Message, "Le nom n'est pas correct","Le message d'erreur n'est pas correct");
    }




    [TestMethod]
    public void ValidationTest()
    {
        // Arrange
        Produit produit = new Produit("TOTO", 1000M, App.Services);
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
