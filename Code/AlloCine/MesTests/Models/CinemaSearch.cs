using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace MesTests
{
    internal class CinemaSearch : ICinemaSearch
    {
       
        public string NomPart { get; set; }
        public string CodePostal { get; set; }
        public int Page { get ; set ; }
        public int NbItemsPerPage { get ; set ; }
    }
}
