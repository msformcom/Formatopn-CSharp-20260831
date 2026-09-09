namespace AlloCineDAL
{
    public class CinemaDAO
    {
        // Id spécifique à la BDD
        public Guid Id { get; set; } = Guid.NewGuid();// génération d'un Id aléatoire 1/340 000 000 000 000 000 000 000 000 000 de doublons dans une BDD avec 1 000 000 000 d'enregistrements

        public string Code { get; set; }
        public string Name { get; set; }

        public string PostalCode { get; set; }

        public int RoomCount { get; set; }

        // Infos sensibles
        public string OwnerName { get; set; }

        // Infos de gestion non utilisées par l'application
        public DateTime LastUpdate { get; set; }

        public ICollection<SeanceDAO> Seances { get; set; }
    }
}
