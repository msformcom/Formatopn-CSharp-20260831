using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Boutique
{
    public class Catalogue : ICatalogue
    {
        internal Catalogue()
        {
            
        }
        /// <summary>
		/// Construit un Catalogue
		/// </summary>
		/// <param name="prixMinimum">le prix minimum des produits du catalogue</param>
		public Catalogue(decimal prixMinimum)
        {
            this.PrixMinimum = prixMinimum;
            //this._ListeProduits = new();
        }

        public int LimiteEpuisementProduit { get; set; } = 10;

        protected List<IProduit> _ListeProduits = new(); // Au lieu de new List<Produit>()

        public IEnumerable<IProduit> ListeProduits
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
        public void AddProduit(IProduit produit, int nbStock = 1)
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
            ((Produit)produit).NbStock = nbStock;
            this._ListeProduits.Add((Produit)produit);
        }

        public decimal VendreProduit(IProduit produit, int qte)
        {
            // Vérification de la validité des paramètres
            if (qte < 0)
            {
                throw new ArgumentException("La quantité vendue ne peut pas être négative");
            }
            if (produit == null)
            {
                throw new ArgumentException("Le produit est null");
            }
            // Existence du produit dans le catalogue
            var p = this._ListeProduits.FirstOrDefault(p => p.Nom == produit.Nom);
            // Si le produit n'existe pas dans le catalogue, on lève une exception
            if (p == null)
            {
                throw new ArgumentException("Le produit n'est pas dans le catalogue");
            }
            // Vérification de la quantité vendue par rapport au stock disponible
            if (qte > p.NbStock)
            {
                throw new ArgumentException("La quantité vendue dépasse le stock disponible");
            }
            // 

            ((Produit)p).NbStock -= qte;
            // Avertir les abonnés que le produit a été vendu
            // En passant les informations utiles aux gestionnaires
            // Test pour savoir si l'évènement a des abonnés
            OnProduitVendu(qte, p, p.Prix * qte);
            if (
                p.NbStock + qte > this.LimiteEpuisementProduit
                &&
                p.NbStock < this.LimiteEpuisementProduit)
            {
                OnProduitPresqueEpuise(new ProduitPresqueEpuiseEventArgs()
                {
                    Limite = this.LimiteEpuisementProduit,
                    Produit = p,
                    StockActualise = p.NbStock,
                    Date = DateTime.Now
                });
            }

            return p.Prix * qte;
        }
        #region Evènements


        #region Evènement ProduitPresqueEpuise
        public event EventHandler<ProduitPresqueEpuiseEventArgs> ProduitPresqueEpuise;

        protected virtual void OnProduitPresqueEpuise(ProduitPresqueEpuiseEventArgs e)
        {
            var handler = ProduitPresqueEpuise;
            if (handler == null) return;

            foreach (Delegate singleHandler in handler.GetInvocationList())
            {
                try
                {
                    ((EventHandler<ProduitPresqueEpuiseEventArgs>)singleHandler)(this, e);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erreur lors de l'exécution d'un gestionnaire pour {nameof(ProduitPresqueEpuise)}: {ex.Message}");
                }
            }
        }
        #endregion


        #region Evènement ProduitVendu

        // protected => visible dans le projet et dans les classes dérivées (héritage)
        // virtual => peut être surchargée dans les classes dérivées (héritage)
        protected virtual void OnProduitVendu(int qte, IProduit produit, decimal montantVente)
        {             // Avertir les abonnés que le produit a été vendu
            // Test pour savoir si l'évènement a des abonnés
            if (ProduitVendu != null)
            {
                // Execution de l'évènement ProduitVendu pour exécuter les gestionnaires de l'évènement
                // Peut ëtre bloquant
                // Couplage faible : le catalogue ne sait pas ce que font les abonnés à l'évènement
                ProduitVendu(this, new ProduitVenduEventArgs(qte, produit, montantVente));
            }
        }

        // Cet évènement va servir à avertir les abonnés que le produit a été vendu
        // Des fonctions de type EventHandler peuvent s'abonner à cet évènement pour être exécutées lorsqu'un produit est vendu
        // EventHandler void EnvoiMail(objet o, EventArgs e)
        // EventHandler<ProduitVenduEventArgs> void EnvoiMail(objet o, ProduitVenduEventArgs e)
        // o => l'objet qui a déclenché l'évènement (ici le catalogue)
        // e=> les arguments de l'évènement (ici aucun)
        public event EventHandler<ProduitVenduEventArgs> ProduitVendu;
        #endregion

        #endregion

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
