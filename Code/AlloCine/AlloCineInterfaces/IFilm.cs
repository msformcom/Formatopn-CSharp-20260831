namespace AlloCineInterfaces
{
    public interface IFilm
    {
        string Code { get; set; }
        string Titre { get; set; }

        int  Duree { get; set; }

        DateOnly DateSortie { get; set; }
    }
}