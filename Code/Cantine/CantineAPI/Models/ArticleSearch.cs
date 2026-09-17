using CantineInterfaces;
using System.ComponentModel.DataAnnotations;

namespace CantineAPI.Models
{
    // Implementation concrete de IArticleSearch, liee depuis le corps de la requete
    public class ArticleSearch : IArticleSearch, IValidatableObject
    {
        public string? SearchText { get; set; }

        [Display(Name = "Prix minimum")]
        [Range(0, 1000)]
        public decimal? PrixMin { get; set; }

        [Display(Name = "Prix maximum")]
        [Required]
        [Range(0, 1000)]
        public decimal? PrixMax { get; set; }

        [Display(Name = "Numero de page")]
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Display(Name = "Taille de page")]
        [Range(1, 100)]
        public int TaillePage { get; set; } = 20;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PrixMin.HasValue && PrixMax.HasValue && PrixMin > PrixMax)
            {
                yield return new ValidationResult(
                    $"Le prix minimum ({PrixMin:C2}) doit etre inferieur au prix maximum ({PrixMax:C2})",
                    new[] { nameof(PrixMin), nameof(PrixMax) });
            }
        }
    }
}
