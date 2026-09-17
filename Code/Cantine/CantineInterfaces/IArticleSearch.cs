namespace CantineInterfaces
{
    public interface IArticleSearch
    {
        public string? SearchText { get; set; }
        public decimal? PrixMin { get; set; }
        public decimal? PrixMax { get; set; }

        // Pagination
        public int Page { get; set; }
        public int TaillePage { get; set; }
    }
}