using CantineInterfaces;

namespace CantineServiceFromBDD
{
    public class CantineServiceBDD : ICantineService
    {
        public Task ConsommerArticle(string matricule, string referenceArticle)
        {
            throw new NotImplementedException();
        }

        public Task IncrementerCreditEmploye(string matricule, decimal montant)
        {
            throw new NotImplementedException();
        }

        public Task<IEmploye> LireEmployeInfos(string matricule)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IArticle>> ListeArticlesAsync(IArticleSearch search)
        {
            throw new NotImplementedException();
        }

        public Task SupprimerArticle(string referenceArticle)
        {
            throw new NotImplementedException();
        }
    }
}
