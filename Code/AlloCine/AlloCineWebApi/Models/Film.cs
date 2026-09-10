using AlloCineInterfaces;

namespace AlloCineWebApi.Models
{
    public class Film : IFilm
    {
        public string Code { get ; set ; }
        public string Titre { get ; set ; }
        public int Duree { get ; set ; }
        public DateOnly DateSortie { get ; set ; }
    }
}
