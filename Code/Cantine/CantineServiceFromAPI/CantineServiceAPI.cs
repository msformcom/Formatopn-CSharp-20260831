using System.Net.Http.Json;
using CantineInterfaces;
using CantineServiceFromAPI.Models;

namespace CantineServiceFromAPI
{
    // Implementation de ICantineService qui delegue le travail a l'API HTTP
    // au lieu d'attaquer la BDD directement
    public class CantineServiceAPI : ICantineService
    {
        private readonly HttpClient http;

        // Le HttpClient est fourni par injection de dependance (typed client)
        // Son BaseAddress est configure par l'appelant
        public CantineServiceAPI(HttpClient http)
        {
            this.http = http;
        }

        public async Task ConsommerArticleAsync(string matriculeEmploye, string referenceArticle, int quantite = 1)
        {
            var reponse = await http.PostAsJsonAsync("api/achats", new ConsommationRequest()
            {
                MatriculeEmploye = matriculeEmploye,
                ReferenceArticle = referenceArticle,
                Quantite = quantite
            });

            if (!reponse.IsSuccessStatusCode)
            {
                // Le corps de la reponse porte le message metier renvoye par l'API
                // (stock insuffisant, credit insuffisant, reference inconnue...)
                var message = await reponse.Content.ReadAsStringAsync();
                throw new ArgumentException(message);
            }
        }

        public async Task<IEnumerable<IArticle>> ListeArticlesAsync(IArticleSearch search)
        {
            var reponse = await http.PostAsJsonAsync("api/articles/search", search);
            reponse.EnsureSuccessStatusCode();

            // L'API renvoie du JSON : le resultat est une liste en memoire,
            // pas un IQueryable comme du cote BDD
            var articles = await reponse.Content.ReadFromJsonAsync<List<Article>>();
            return articles!;
        }

        public async Task<IEmploye> LireEmployeInfosAsync(string matricule)
        {
            var reponse = await http.GetAsync($"api/employes/{matricule}");
            reponse.EnsureSuccessStatusCode();

            var employe = await reponse.Content.ReadFromJsonAsync<Employe>();
            return employe!;
        }

        public Task SupprimerArticleAsync(string referenceArticle)
        {
            throw new NotImplementedException();
        }

        public Task IncrementerCreditEmployeAsync(string matricule, decimal montant)
        {
            throw new NotImplementedException();
        }
    }
}
