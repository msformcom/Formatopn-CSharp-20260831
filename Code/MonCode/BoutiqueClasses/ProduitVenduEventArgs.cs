using System;
using System.Collections.Generic;
using System.Text;

namespace Boutique
{
    public class ProduitVenduEventArgs : EventArgs
    {
        public ProduitVenduEventArgs(int quantite, Produit produit, Decimal montantVente)
        {
            Quantite = quantite;
            Produit = produit;
            MontantVente = montantVente;
        }
        public int Quantite { get; set; }
        public Produit Produit { get; set; }
        public Decimal MontantVente  { get; set; }
    }
}
