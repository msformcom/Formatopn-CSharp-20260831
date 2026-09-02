using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Boutique
{
    public class Catalogue
    {
        /// <summary>
		/// Construit un Catalogue
		/// </summary>
		/// <param name="prixMinimum">le prix minimum des produits du catalogue</param>
		public Catalogue(decimal prixMinimum)
        {
            this.PrixMinimum = prixMinimum;
            //this._ListeProduits = new();
        }

        protected List<Produit> _ListeProduits = new(); // Au lieu de new List<Produit>()

        public IEnumerable<Produit> ListeProduits
        {
            get
            {
                return this._ListeProduits.Where(c => c.NbStock > 0).Take(100);
            }
        }

        /// <summary>
        /// Ajoute un produit au catalogue
        /// </summary>
        /// <param name="produit">Le produit à ajouter</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddProduit(Produit produit)
        {
            if (produit == null)
            {
                throw new ArgumentNullException("Le produit est null");
            }
            if (this._ListeProduits.Any(p => p.Nom == produit.Nom))
            { 
                throw new ArgumentException($"Le produit {produit.Nom} existe déjà dans le catalogue.");
            }
            if (produit.Prix < this.PrixMinimum)
            {
#if DEBUG
                Debug.WriteLine("Le prix du produit {0} est inférieur au prix minimum du catalogue {1:C}.", produit.Nom, this.PrixMinimum);
#endif
                throw new ArgumentException($"Le prix du produit {produit.Nom} est inférieur au prix minimum du catalogue {this.PrixMinimum:C}.");
            }
            this._ListeProduits.Add(produit);
        }


        #region Propriété PrixMinimum

        private decimal _PrixMinimum;

        public decimal PrixMinimum
        {
            get { return _PrixMinimum; }
            private set
            {
                // TODO : Ajoutez ici la logique de validation pour la propriété PrixMinimum
                if (value <= 0)
                {
                    throw new ArgumentException("Le prix minimum ne peut être inférieur à 0");
                }
                _PrixMinimum = value;
            }
        }
        #endregion

    }
}
