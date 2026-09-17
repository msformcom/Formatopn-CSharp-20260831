using System.Net;
using CantineInterfaces;

namespace CantineServiceFromAPI
{
    public class CantineServiceAPI : ICantineService
    {
        private readonly HttpClient httpClient;

        public CantineServiceAPI(HttpClient httpClient)
        {
            // La configuration du client (adresse du service)
            // est faite par DI
            this.httpClient = httpClient;
        }
        public async Task ConsommerArticleAsync(string matriculeEmploye, string referenceArticle, int quantite = 1)
        {
            try
            {
                var reponseServer = await httpClient.GetAsync($"/Cantine/AddAchat?matriculeEmploye={matriculeEmploye}&referenceArticle={referenceArticle}&quantite={quantite}");
            // Envoyer une requete http correcte au serveur et attendre les résultat
                if (reponseServer.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("L'opération a échoué");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("L'opération a échoué");
            }

        }

        public Task IncrementerCreditEmployeAsync(string matricule, decimal montant)
        {
            throw new NotImplementedException();
        }

        public Task<IEmploye> LireEmployeInfosAsync(string matricule)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IArticle>> ListeArticlesAsync(IArticleSearch search)
        {
            throw new NotImplementedException();
        }

        public Task SupprimerArticleAsync(string referenceArticle)
        {
            throw new NotImplementedException();
        }
    }
}
