using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Boutique
{
    internal class Catalogue
    {
        public Catalogue(decimal prixMinimum)
        {
            this.PrixMinimum = prixMinimum;
			//this._ListeProduits = new();
        }

		protected List<Produit> _ListeProduits=new(); // Au lieu de new List<Produit>()

		public IEnumerable<Produit> ListeProduits
		{
			get { 
				return this._ListeProduits.Where(c=>c.NbStock>0).Take(100); 
			}
		}


		public void AddProduit(Produit produit)
		{
			if(produit.Prix < this.PrixMinimum)
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
				if (value<=0)
				{
					throw new ArgumentException("Le prix minimum ne peut être inférieur à 0");
				}
				_PrixMinimum = value;
			}
		}
		#endregion

	}
}
