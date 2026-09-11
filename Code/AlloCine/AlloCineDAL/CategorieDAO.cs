using System;
using System.Collections.Generic;
using System.Text;

namespace AlloCineDAL
{
    public  class CategorieDAO
    {
        public Guid Id { get; set; } = Guid.NewGuid();// génération d'un Id aléatoire 1/340 000 000 000 000 000 000 000 000 000 de doublons dans une BDD avec 1 000 000 000 d'enregistrements

        public string Code { get; set; }
        public string Label { get; set; }

        public ICollection<FilmDAO> Films { get; set; } = new HashSet<FilmDAO>();
    }
}
