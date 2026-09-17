using CantineInterfaces;

namespace CantineServiceFromBDD.Models
{
    public class Employe : IEmploye
    {

        public string Matricule { get; set; }
        public DateOnly DateNaissance { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}
