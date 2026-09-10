

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
