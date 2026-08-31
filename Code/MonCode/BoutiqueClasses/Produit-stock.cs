using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique
{
    // Modificateur access classe
    // internal => visible uniquement dans le projet
    // public => visible dans tous les projets
    // partial => permet de scinder la définition d'une classe sur plusieurs fichiers
   
    internal  partial class Produit
    {

        public int  NbStock;
    }
}
