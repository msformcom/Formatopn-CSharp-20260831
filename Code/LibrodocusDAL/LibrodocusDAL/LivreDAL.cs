namespace LibrodocusDAL
{
    // class DAL Data Access Layer => Sert à stocker les données
    // class DTO Data Transfert Object => Sert à transporter des information
    // Class POCO / POJO => Plain Old CLR Object / Plain Old Java Object => Class de l'appli
    public class LivreDAL
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // => PrimaryKey
        public string ISBN { get; set; } // VARCHAR(20) => ASCII => 1 car => 1 octet => unique
        public string Titre { get; set; } // NVARCHAR(100) => Unicode => 1 car => 1 octet ou é ou 3 ou 4
        public string Resume { get; set; }
        public Byte[]? PremierDeCouvertue { get; set; }

        // Propriété de navigation
        // 1) Préciser que 1 livre => 1 Genre
        // 2) Permet de lire (dans la BDD) le genre du livre

        public Guid IdGenre { get; set; }
        public GenreDAL? Genre { get; set; }

    }
    //db.livres.Include(l=>l.Genre).Include(g=>g.Livres)
}
