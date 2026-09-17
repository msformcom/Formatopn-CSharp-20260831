using System.Net.Http.Json;
using CantineInterfaces;
using CantineApiContracts;
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

        // Toutes les reponses de l'API sont enveloppees de la meme facon :
        // le message metier de l'enveloppe devient une exception
        private static async Task VerifierAsync(HttpResponseMessage reponse)
        {
            var resultat = await reponse.Content.ReadFromJsonAsync<ResponseWrapper>();
            if (resultat == null || !resultat.Success)
            {
                throw new ArgumentException(resultat?.Message ?? reponse.ReasonPhrase);
            }
        }

        private static async Task<T> DeballerAsync<T>(HttpResponseMessage reponse)
        {
            var resultat = await reponse.Content.ReadFromJsonAsync<ResponseWrapper<T>>();
            if (resultat == null || !resultat.Success || resultat.Data == null)
            {
                throw new ArgumentException(resultat?.Message ?? reponse.ReasonPhrase);
            }

            return resultat.Data;
        }

        public async Task ConsommerArticleAsync(string matriculeEmploye, string referenceArticle, int quantite = 1)
        {
            var reponse = await http.PostAsJsonAsync("api/achats", new ConsommationRequest()
            {
                MatriculeEmploye = matriculeEmploye,
                ReferenceArticle = referenceArticle,
                Quantite = quantite
            });

            // Le 400 porte une enveloppe exploitable
            // (stock insuffisant, credit insuffisant, reference inconnue...)
            await VerifierAsync(reponse);
        }

        public async Task<IEnumerable<IArticle>> ListeArticlesAsync(IArticleSearch search)
        {
            var reponse = await http.PostAsJsonAsync("api/articles/search", search);
            // Une erreur non prevue par l'API arrive sans enveloppe
            reponse.EnsureSuccessStatusCode();

            // L'API renvoie du JSON : le resultat est une liste en memoire,
            // pas un IQueryable comme du cote BDD
            return await DeballerAsync<List<Article>>(reponse);
        }

        public async Task<IEmploye> LireEmployeInfosAsync(string matricule)
        {
            var reponse = await http.GetAsync($"api/employes/{matricule}");
            reponse.EnsureSuccessStatusCode();

            return await DeballerAsync<Employe>(reponse);
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
