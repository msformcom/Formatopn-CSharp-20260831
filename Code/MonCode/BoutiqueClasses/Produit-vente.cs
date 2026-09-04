using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique
{
    public partial class Produit
    {
        // Par défaut il existe un constructeur sans paramètre

        // On veut forcer l'initialisation du prix à la création de l'instance
        // On crée un constructeur avec un paramètre prix
        // Qui passe le nom au constructeur privé qui gère l'initialisation du nom
        public Produit(string nom, decimal prix, IServiceProvider s) : this(nom,s)
        {

            // On utilise la propriété Prix pour valider la valeur
            this.Prix = prix;
        }

        // private => visible uniquement dans la classe
        // public => visible dans tous les projets
        // internal => visible uniquement dans le projet

        // Champs => espace de stockage
        private decimal _Prix = 10000000000;
        // Valeur par défaut : toujours OK

        // Propriété => interface de communication avec le monde extérieur
        public decimal Prix
        {
            get {
                // Permet de lire la valeur stockée dans le champ _Prix
                return _Prix; }
            // Visible dans le projet et dans les classes dérivées (héritage)
            internal protected set
            {
                // permet de modifier la valeur stockée dans le champ _Prix
                // avant d'affecter _Prix, on valide la nouvelle valeur = value
                if (value <= 0)
                {
                    // si la nouvelle valeur est incorrecte, on lève une exception
                    throw new ArgumentException("Le prix ne peut pas être négatif");
                }
                _Prix = value;
            }
        }

    }
}
