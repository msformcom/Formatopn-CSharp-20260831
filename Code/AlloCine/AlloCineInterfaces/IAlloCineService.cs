namespace AlloCineInterfaces
{
    public interface IAlloCineService
    {
        // La lecture des cinémas passe par des processus (internet) qui peuvent être longs
        // On ne peut l'utiliser comme une fonction "normale"
        Task<IEnumerable<ICinema>> GetCinemasAsync(string codePostal);
        Task<IEnumerable<IFilm>> GetFilmsByCinemaAsync(string codeCinema);

        Task<IEnumerable<ICinema>> GetCinemasByFilmAsync(string codeFilm);

    }
}
