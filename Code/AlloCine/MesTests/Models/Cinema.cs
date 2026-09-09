using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace MesTests
{
    internal class Cinema : ICinema
    {
        public string Code { get; set; }
        public string Nom { get; set; }
        public string CodePostal { get; set; }
        public int NombreSalles { get; set; }
    }
}
