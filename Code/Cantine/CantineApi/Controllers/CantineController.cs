using CantineInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CantineApi.Controllers
{
    // Utilisé si l'URL est de la forme /Cantine
    [Route("[controller]")]
    [ApiController]
    public class CantineController : ControllerBase, ICantineService
    {
        private readonly ICantineService service;

        public CantineController(ICantineService service)
        {
            this.service = service;
        }

        // POST : /Cantine/AddAchat
        [HttpGet("AddAchat")]
        public async  Task ConsommerArticleAsync(string matriculeEmploye, string referenceArticle, int quantite = 1)
        {
            try
            {
                await service.ConsommerArticleAsync(matriculeEmploye, referenceArticle, quantite);
            }
            catch (Exception)
            {

                throw new HttpRequestException("Sorry");
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
