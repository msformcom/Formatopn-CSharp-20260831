using System;
using System.Collections.Generic;
using System.Text;

namespace HRDAL.DAO
{
    // Permet de définir la structure des informations stockées
    public class ArticleDAO
    {
        // Nécessaire à la BDD
        public Guid Id { get; set; } = Guid.NewGuid();
        // Guid => Génération offline simple, id non prévisibles
        // protection contre les sinistres 

        // Données nécessaires au métier de notre appli
        public string Label { get; set; }
        public decimal Price { get; set; }

        public string Reference { get; set; }

        public byte[] Photo { get; set; }

        public string Allergens { get; set; } // => "gluten","arsenic"

        // Données d'une autre appli ou sensibles
        public decimal Marge { get; set; }

        // Données de gestion
        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

    }
}
