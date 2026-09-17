using CantineInterfaces;

namespace CantineAPI.Models
{
    // Contrat de sortie de l'API pour un article
    // Independant de IArticle : le metier peut evoluer sans changer le JSON
    public class ArticleDto
    {
        public string Reference { get; set; } = string.Empty;
        public string Libelle { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        public byte[]? Photo { get; set; }
        public ICollection<string> Allergenes { get; set; } = new List<string>();

        public static ArticleDto Depuis(IArticle article) => new ArticleDto()
        {
            Reference = article.Reference,
            Libelle = article.Libelle,
            Prix = article.Prix,
            Photo = article.Photo,
            Allergenes = article.Allergenes
        };
    }
}
