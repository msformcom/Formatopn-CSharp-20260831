using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace AlloCineServiceApi
{
    internal class Film : IFilm
    {
        
        public string Code { get; set; }
        public string Titre { get; set; }
        public int Duree { get; set; }
        public DateOnly DateSortie { get; set; }
    }
}
