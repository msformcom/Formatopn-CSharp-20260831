namespace AlloCineInterfaces
{
    public interface IAlloCineService
    {
        // La lecture des cinémas passe par des processus (internet) qui peuvent être longs
        // On ne peut l'utiliser comme une fonction "normale"
        Task<IEnumerable<ICinema>> GetCinemasAsync(ICinemaSearch search);

        // Recherche des films dont le titre contient un texte donné
        Task<IEnumerable<IFilm>> SearchFilmsAsync(IFilmSearch search); 
        Task<IEnumerable<IFilm>> GetFilmsByCinemaAsync(string codeCinema);

        Task<IEnumerable<ICinema>> GetCinemasByFilmAsync(string codeFilm);

        Task<IFilm> AddFilmAsync(IFilm film);
        Task<ICinema> AddCinemaAsync(ICinema cinema);
        Task<ISeance> AddSeanceAsync(ISeance seance);

        Task<ICategorie> GetCategoryByFilmAsync(string codeFilm);
       
        // Task RemoveFilm(IFilm film);

    }
}
