using System;
using System.Collections.Generic;
using System.Text;

namespace Boutique.BDD
{
    public class CatalogueBDD : ICatalogue
    {
        public int LimiteEpuisementProduit { get; set; }

        public IEnumerable<IProduit> ListeProduits => throw new NotImplementedException();

        public decimal PrixMinimum { get; set; }

        public event EventHandler<ProduitPresqueEpuiseEventArgs> ProduitPresqueEpuise;
        public event EventHandler<ProduitVenduEventArgs> ProduitVendu;

        public void AddProduit(IProduit produit, int nbStock = 1)
        {
            throw new NotImplementedException();
        }

        public decimal VendreProduit(IProduit produit, int qte)
        {
            throw new NotImplementedException();
        }
    }
}
