using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace MesTests.Models
{
    internal class FilmSearch : IFilmSearch
    {
        public int? DureeMin { get ; set ; }
        public int? DureeMax { get ; set ; }
        public string? TitrePart { get ; set ; }
        public int Page { get ; set ; }
        public int NbItemsPerPage { get ; set ; }
    }
}
