using CantineInterfaces;

namespace CantineAPI.Models
{
    // Implementation concrete de IArticleSearch, liee depuis la query string
    public class ArticleSearch : IArticleSearch
    {
        public string? SearchText { get; set; }
        public decimal? PrixMax { get; set; }
    }
}
