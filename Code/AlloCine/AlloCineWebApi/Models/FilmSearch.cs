using AlloCineInterfaces;

namespace AlloCineWebApi.Models
{
    public class FilmSearch : IFilmSearch
    {
        public int? DureeMin { get ; set ; }
        public int? DureeMax { get ; set ; }
        public string? TitrePart { get ; set ; }
        public int Page { get ; set ; }
        public int NbItemsPerPage { get ; set ; }
    }
}
