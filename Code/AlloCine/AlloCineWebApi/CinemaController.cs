

using AlloCineInterfaces;
using AlloCineWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlloCineWebApi
{

    // Une classe de controller permet de mettre à disposition sur le réseau
    // des méthode implémentée dans la classe
    // Ce controller sera choisi si la route(interprétaion de l'url)
    // commance par /Cinema
    [Route("Cinema")]
    // Cette classe est une classe de controller Api
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly IAlloCineService service;

        public CinemaController(
            ILogger<CinemaController> logger,
            IAlloCineService service
            
            )
        {
            logger.LogInformation("Instanciation de CinemaController");
            this.service = service;
        }


        [HttpPost()]
        // POST : /Cinema
        // Binder => Inspecte la requete => lit le Body => Déserialise en Cinema
        // Puis utilise les attributs de validation sur la classe Cinema
        // Pour valider => ModelState
        public async Task<ActionResult<ICinema>> AddCinema([FromBody] Cinema cinema)
        {
            // La classe Cinema peut avoir des attributs de validation 
            if (!this.ModelState.IsValid)
            {
                // retour d'érreur de requete => indique à l'utilisateur pourquoi le Cinema n'était pas correct
                return BadRequest(this.ModelState);
            }
            var cineDansBDD = await service.AddCinemaAsync(cinema);
            return Ok(cineDansBDD); // StatusCode 200 + Json 
        }

        // GET /Cinema?page=1&nbElementParPage=10
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Cinema>>> SearchCinema(CinemaSearch search, 
                                                                        int page=1, 
                                                                        int nbElementParPage=10 )
        {
            if (!this.ModelState.IsValid)
            {
                // retour d'érreur de requete => indique à l'utilisateur pourquoi le Cinema n'était pas correct
                return BadRequest(this.ModelState);
            }
            var cinemas = await service.GetCinemasAsync(search);
            cinemas=cinemas.Skip((page-1)*nbElementParPage).Take(nbElementParPage);
            // Skip et Take sont par défaut les méthodes de IEnumerable => pas traduit dans le SQL
            // Sauf si IQueryable (ProjectTo) => SELECT .... OFFSET 10 FETCH NEXT 10 ROWS ONLY
            return Ok(cinemas); // StatusCode 200 + Json 
        }
        

        
        //[HttpGet("Add/{a:int}/{b:int}")]
        //[HttpGet("Ajouter/{a:int}/{b:int}")]
        //// GET /Cinema/Add?a=1&b=2
        ////Binder => Objet qui recherche les paramètres de la fonction
        //// Query, Body, Services, Cookies, Header
        //public int Addition(int a=0,  int b=0)
        //{
        //    return a + b;
        //}
    }
}
