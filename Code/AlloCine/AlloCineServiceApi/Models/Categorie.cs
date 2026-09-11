using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace AlloCineServiceApi
{
    public class Categorie : ICategorie
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }
}
