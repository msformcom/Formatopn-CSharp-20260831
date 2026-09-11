using System.Net.Http.Json;
using AlloCineInterfaces;

namespace AlloCineServiceApi
{
    public class AlloCineServiceFromApi : IAlloCineService
    {
        private readonly HttpClient httpClient;

        // Je veux un client HTTP configuré (Avec l'adresse de l'api)
        public AlloCineServiceFromApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public Task<ICinema> AddCinemaAsync(ICinema cinema)
        {
            throw new NotImplementedException();
        }

        public Task<IFilm> AddFilmAsync(IFilm film)
        {
            throw new NotImplementedException();
        }

        public Task<ISeance> AddSeanceAsync(ISeance seance)
        {
            throw new NotImplementedException();
        }

        public async Task<ICategorie> GetCategoryByFilmAsync(string codeFilm)
        {
            
            return  await httpClient.GetFromJsonAsync<Categorie>($"Categorie/GetByFilm/{codeFilm}");
        }

        public async Task<IEnumerable<ICinema>> GetCinemasAsync(ICinemaSearch search)
        {
            // Interroger l'api et exécuter la bonne méthode
            var request = new HttpRequestMessage(HttpMethod.Get, "Cinema")
            {
                Content = JsonContent.Create(search)
            };

            // Envoi
            var response = await httpClient.SendAsync(request);
            return (await response.Content.ReadFromJsonAsync<IEnumerable<Cinema>>())!;


        }

        public Task<IEnumerable<ICinema>> GetCinemasByFilmAsync(string codeFilm)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFilm>> GetFilmsByCinemaAsync(string codeCinema)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFilm>> SearchFilmsAsync(IFilmSearch search)
        {
            throw new NotImplementedException();
        }
    }
}
