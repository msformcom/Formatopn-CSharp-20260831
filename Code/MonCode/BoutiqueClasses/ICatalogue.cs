namespace Boutique
{

    //public interface ICatalogue<T> 
    //{
    //    IEnumerable<T> ListeElements { get; }

    //}

    //public interface ICatalogueProduit : ICatalogue<IProduit> { }

    public interface ICatalogue
    {
        int LimiteEpuisementProduit { get; set; }
        IEnumerable<IProduit> ListeProduits { get; }
        decimal PrixMinimum { get; }

        event EventHandler<ProduitPresqueEpuiseEventArgs> ProduitPresqueEpuise;
        event EventHandler<ProduitVenduEventArgs> ProduitVendu;

        void AddProduit(IProduit produit, int nbStock = 1);
        decimal VendreProduit(IProduit produit, int qte);
    }
}