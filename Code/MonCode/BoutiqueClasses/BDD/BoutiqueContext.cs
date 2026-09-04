using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Boutique.BDD
{
    // BoutiqueContext va faire l'accès à la BDD pour moi
    // Package Microsoft.EntityFrameworkCore
    // DbContext qui contient la mécanique d'accès à la BDD
    public class BoutiqueContext : DbContext
    {
        // Stockage des produits dans une table de la BDD
        public DbSet<ProduitBDD> Produits { get; set; }
    }
}
