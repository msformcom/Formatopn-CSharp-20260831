using System;
using System.Collections.Generic;
using System.Text;

namespace AlloCineInterfaces
{
    public interface IFilmSearch : IEntitySearch
    {
        public int? DureeMin { get; set; }
        public int? DureeMax { get; set; }
        public string?  TitrePart { get; set; }
    }
}
