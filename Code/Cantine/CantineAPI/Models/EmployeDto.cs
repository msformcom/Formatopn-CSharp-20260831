using CantineInterfaces;

namespace CantineAPI.Models
{
    // Contrat de sortie de l'API pour un employe
    public class EmployeDto
    {
        public string Matricule { get; set; } = string.Empty;
        public DateOnly DateNaissance { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;

        public static EmployeDto Depuis(IEmploye employe) => new EmployeDto()
        {
            Matricule = employe.Matricule,
            DateNaissance = employe.DateNaissance,
            Nom = employe.Nom,
            Prenom = employe.Prenom
        };
    }
}
