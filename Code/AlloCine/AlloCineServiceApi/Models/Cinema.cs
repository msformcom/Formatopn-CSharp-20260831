using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace AlloCineServiceApi
{
    internal class Cinema : ICinema
    {
        public Cinema()
        {
                
        }
        public string Code { get; set; }
        public string Nom { get; set; }
        public string CodePostal { get; set; }
        public int NombreSalles { get; set; }
    }
}
