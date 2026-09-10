using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace AlloCineServiceApi
{
    internal class Seance : ISeance
    {
        public DateTime Horaire { get; set; }
        public string Code { get; set; }
        public ICinema Cinema { get; set; }
        public IFilm Film { get; set; }
    }
}
