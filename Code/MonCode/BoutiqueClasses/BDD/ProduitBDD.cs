using System;
using System.Collections.Generic;
using System.Text;

namespace Boutique.BDD
{
    public class ProduitBDD : IProduit
    {
        // Comme on doit connaitre l'id lors de la modif d'un produit
        // on ajoute l'id mais seulement en iternal
        internal Guid Id { get; set; } = Guid.NewGuid();

        // Id du catalogue
        internal Guid IdCatalogue { get; set; }

        public int  NbStock  { get; internal set; }
        // public int NbStock => throw new NotImplementedException(); // <=> public int nbStock{get}

        public Decimal Prix { get; internal set; }

        public string Nom { get; internal set; }

        internal DateTime LastModif { get; set; }=DateTime.Now;
    }
}
