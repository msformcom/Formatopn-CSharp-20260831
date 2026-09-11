using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlloCineDAL
{
    // Cette classe représente la structure des informations dans la BDD

    [Table("TBL_Films")]
    public class FilmDAO
    {
      
        public FilmDAO() {}


        [Key]
        [Column("PK_Film")]
        public Guid Id { get; set; } = Guid.NewGuid();// génération d'un Id aléatoire 1/340 000 000 000 000 000 000 000 000 000 de doublons dans une BDD avec 1 000 000 000 d'enregistrements

        [MaxLength(100)]
        public string Code { get; set; }


        public string Title { get; set; }

        public int Length { get; set; }

        public Guid IdCategorie { get; set; }

        public DateOnly ReleaseDate { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.Now;

        // Indique à EF que un Film est associé à une ICollection de séance
        public ICollection<SeanceDAO> Seances { get; set; }

        public CategorieDAO Categorie { get; set; }


    }
}
