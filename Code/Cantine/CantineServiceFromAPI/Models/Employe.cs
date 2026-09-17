using System;
using System.Collections.Generic;
using System.Text;
using CantineInterfaces;

namespace CantineServiceFromAPI.Models
{
    public class Employe : IEmploye
    {
        public string Matricule { get; set; }
        public DateOnly DateNaissance { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}
