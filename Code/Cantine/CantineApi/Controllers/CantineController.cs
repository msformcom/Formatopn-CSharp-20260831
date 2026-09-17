using CantineApi.CustomAttributes;
using CantineApi.Models;
using CantineInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CantineApi.Controllers
{
    // Utilisé si l'URL est de la forme /Cantine
    [Route("[controller]")]
    [ApiController]
    public class CantineController : ControllerBase
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
            catch (Exception ex)
            {

                throw new HttpRequestException(ex.Message);
            }
            
        }

        public Task IncrementerCreditEmployeAsync(string matricule, decimal montant)
        {
            throw new NotImplementedException();
        }

        //[HttpGet("LireEmploye/{matricule: pattern('[A-Z]{2}[0-9]{3}')}")]
        public Task<IEmploye> LireEmployeInfosAsync([AntiInjection]string matricule)
        {
            throw new NotImplementedException();
        }

        // GET : /Cantine/Articles + body avec objet ArticleSearch
        [HttpGet("Articles")]
        //[HttpPost("Articles")]
        //[ValidateModel]
        // ActionFilter => en MVC l'équivalent des middleware
        // Ajouter par attribut un actionfilter qui va valider le modele
        public async Task<IEnumerable<IArticle>> ListeArticlesAsync([FromQuery]ArticleSearch search)
        {
            if (!ModelState.IsValid)
            {
                // ModelState => etat du model => Résultat de la validation de ArticleSearch
                throw new ArgumentException(ModelState.First().Value.Errors.First().ErrorMessage);
            }

            var resultat= await service.ListeArticlesAsync(search);
            if(resultat is IQueryable<IArticle> query)
            {
                resultat=query.Skip((search.PageNumber-1)*search.NbItemPerPage).Take(search.NbItemPerPage);
            }
            else
            {
                resultat = resultat.Skip((search.PageNumber - 1) * search.NbItemPerPage).Take(search.NbItemPerPage);

            }
            return resultat; // Envoye au serializer json => lire les enregistrement au fur et à mesure que le client http lit le flux réseau
        }

        public Task SupprimerArticleAsync(string referenceArticle)
        {
            throw new NotImplementedException();
        }
    }
}
