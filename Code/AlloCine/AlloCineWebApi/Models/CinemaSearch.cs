using AlloCineInterfaces;

namespace AlloCineWebApi.Models
{
    public class CinemaSearch : ICinemaSearch
    {
        public string? CodePostal { get; set; }
        public string? NomPart { get; set; }
        public int Page { get ; set ; }
        public int NbItemsPerPage { get ; set ; }
    }
}
