namespace CantineInterfaces
{
    public interface IArticleSearch
    {
        public string? SearchText { get; set; }
        public decimal? PrixMin { get; set; }
        public decimal? PrixMax { get; set; }
    }
}