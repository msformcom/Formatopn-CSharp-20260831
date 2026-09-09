using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlloCineDAL
{
    public  class SeanceDAO
    {
        public Guid Id { get; set; } = Guid.NewGuid();// génération d'un Id aléatoire 1/340 000 000 000 000 000 000 000 000 000 de doublons dans une BDD avec 1 000 000 000 d'enregistrements
        public string Code { get; set; }

        public DateTime Date { get; set; }


        [ForeignKey(nameof(Cinema))]  // Association avec la table Cinemas
        public Guid IdCinema { get; set; }
        public Guid IdFilm { get; set; }

        // EntityFramework me permet d'ajouter une relation 1-N
        // grace à une propriété de navaigation qui pointe vers le type de l'autre coté de la relation
        public CinemaDAO Cinema { get; set; }
        public FilmDAO Film { get; set; }


    }
}
