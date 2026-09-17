using System.ComponentModel.DataAnnotations;
using CantineInterfaces;

namespace CantineApi.Models
{
    public class ArticleDTO : IArticle
    {

        public string Reference { get ; set ; }
        public string Libelle { get ; set ; }

        // Utilisé pour valider les DTO entrants
        // Mais aussi coté client si le formulaire supporte ce type d'attributs (WPF)
        // Cette classe sera commune au client et au server (
        [Range(1,100000, ErrorMessage ="Le {0} doit être entre {1} et {2}")]
        public decimal Prix { get ; set ; }
        public byte[] Photo { get ; set ; }

        public ICollection<string> Allergenes { get; set; }
    }
}
