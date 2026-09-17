using System.ComponentModel.DataAnnotations;
using CantineApi.CustomAttributes;
using CantineInterfaces;

namespace CantineApi.Models
{
    // Cette classe va service à recevoir les données de mon client
    // Les attributs de validation valident chaque donnée reçue
    public class ArticleSearchDTO : IArticleSearch, IValidatableObject
    {
         private string _SearchText="Toto";

        public string SearchText
        {
            get { 
                return _SearchText; }
            set { 
                _SearchText = value; 
            }
        }


        [Required(ErrorMessage = "Le {0} est requis")]
        [Range(1, 100000, ErrorMessage = "Le {0} doit être compris entre {1} et {2}")]
        [Display(Name = "Prix maximum")]
        public decimal? PrixMax { get; set; } = 100000;

        public int PageNumber { get; set; } = 1;
        public int NbItemPerPage { get; set; } = 10;

        public decimal? PrixMin { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PrixMax !=null && PrixMin !=null &&  PrixMax < PrixMin)
            {
                yield return new ValidationResult($"Le {nameof(PrixMin)} doit être inférieur au {nameof(PrixMax)}", new List<string>() { nameof(PrixMax), nameof(PrixMin) });
            }
        }
    }
}
