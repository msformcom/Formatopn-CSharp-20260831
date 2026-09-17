using System.ComponentModel.DataAnnotations;

namespace CantineAPI.Models
{
    // Corps de la requete POST api/achats
    public class ConsommationRequest
    {
        [Display(Name = "Matricule employe")]
        [Required]
        public string MatriculeEmploye { get; set; } = string.Empty;

        [Display(Name = "Reference article")]
        [Required]
        public string ReferenceArticle { get; set; } = string.Empty;

        [Display(Name = "Quantite")]
        [Range(1, int.MaxValue)]
        public int Quantite { get; set; } = 1;
    }
}
