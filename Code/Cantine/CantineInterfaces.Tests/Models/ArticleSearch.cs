namespace CantineInterfaces.Tests.Models
{
    public class ArticleSearch : IArticleSearch
    {
        public string? SearchText { get ; set ; }
        public decimal? PrixMin { get ; set ; }
        public decimal? PrixMax { get ; set ; }
    }
}
