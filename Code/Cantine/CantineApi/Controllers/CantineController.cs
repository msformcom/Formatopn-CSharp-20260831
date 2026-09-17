using AutoMapper.Configuration.Annotations;
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
        //[HttpGet("Articles")]
        [HttpPost("Articles/ListeArticles")]
        //[Mapping(typeof(IEnumerable<ArticleDTO>)]
        //[ValidateModel]
        // ActionFilter => en MVC l'équivalent des middleware
        // Ajouter par attribut un actionfilter qui va valider le modele
        public async Task<IEnumerable<IArticle>> ListeArticlesAsync([FromBody]ArticleSearchDTO search)
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
            // Je retourne les résultats dans un DTO
            // car sinon, je passe l'objet provenant du service (ICantineServiceBDD)
            // directement au client => problème car le service peut ajouter dans la classe 
            // qu'il utilise des information
            return resultat.Select(model=>new ArticleDTO()
            {
                Allergenes = model.Allergenes,
                Libelle =   model.Libelle,
                Prix=   model.Prix,
                Photo= model.Photo,
                Reference    =model.Reference
            }); // Envoye au serializer json => lire les enregistrement au fur et à mesure que le client http lit le flux réseau
        }

        public Task SupprimerArticleAsync(string referenceArticle)
        {
            throw new NotImplementedException();
        }
    }
}
