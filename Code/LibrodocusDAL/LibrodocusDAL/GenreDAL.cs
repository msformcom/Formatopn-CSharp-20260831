using System;
using System.Collections.Generic;
using System.Text;

namespace LibrodocusDAL
{
    public class GenreDAL
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Libelle { get; set; }

        public ICollection<LivreDAL> Livres { get; set; } = new HashSet<LivreDAL>();
    }

    // db.Genres => SELECT * FROM Genres  => Livre null
    // 
    // db.Genres.Include(g=>g.Livres) SELECT * FROM Genres INNER JOIN Livres...
    
}
