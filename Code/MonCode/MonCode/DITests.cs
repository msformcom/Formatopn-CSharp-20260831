using Microsoft.Extensions.DependencyInjection;

namespace MonCode;

[TestClass]
public class DITests
{
    [TestMethod]
    public void CreationServices()
    {
        var serviceCollection = new ServiceCollection();
        // Ajout et config des service de l'injection

        // Association dans les service entre une demande de ICatalogue
        // Avec la création d'une instance nouvelle de Catalogue

#if VERSIONLIMITEE
        serviceCollection.AddTransient<ICatalogue,Catalogue>(s=>new Catalogue(10));
#else
        serviceCollection.AddKeyedTransient<IProduit>("Fourchette",(s,o)=>new Produit("FOURCHETTE",12,App.Services));
        serviceCollection.AddKeyedTransient<IProduit>("Couteau", (s, o) => new Produit("COUTEAU", 23, App.Services));
        serviceCollection.AddTransient<ICatalogue>(s=> {
            ICatalogue c=new Catalogue(5);
            c.AddProduit(s.GetRequiredKeyedService<IProduit>("Fourchette"),10);
            c.AddProduit(s.GetRequiredKeyedService<IProduit>("Couteau"), 10);
            c.AddProduit(new Produit("ASSIETTE", 76, App.Services));

            return c;
        });
#endif


        // Création de l'injecteur
        var di =serviceCollection.BuildServiceProvider();

        var catalogue = di.GetService<ICatalogue>();
    }
}
