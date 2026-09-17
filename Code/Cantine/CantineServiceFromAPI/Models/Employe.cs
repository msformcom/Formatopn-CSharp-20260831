using CantineInterfaces;

namespace CantineServiceFromAPI.Models
{
    public class Employe : IEmploye
    {
        public string Matricule { get; set; } = string.Empty;
        public DateOnly DateNaissance { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
    }
}
