using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRDAL.DAO

{
    // Permet de définir la structure des informations stockées
    
    //[Table("TBL_Articles")]
    public class ArticleDAO
    {
        // Nécessaire à la BDD
        //[Key]
        //[Column("PK_Article")]
        public Guid Id { get; set; } = Guid.NewGuid();
        // Guid => Génération offline simple, id non prévisibles
        // protection contre les sinistres 

        // Données nécessaires au métier de notre appli
        // Gestion des spécifications dans la BDD par le biais d'attributs
        // Abandonné => au profit fluentApi
        //[MaxLength(100)] 
        public string Label { get; set; }
        public decimal Price { get; set; }

        public string Reference { get; set; }

        public byte[]? Photo { get; set; }

        public string? Allergens { get; set; } // => "gluten","arsenic"

        // Données d'une autre appli ou sensibles
        public decimal Marge { get; set; }

        // Données de gestion
        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

    }
}
