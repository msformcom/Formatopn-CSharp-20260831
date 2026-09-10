using AlloCineInterfaces;
using AlloCineWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace AlloCineWebApi
{
    [Route("[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        public FilmController()
        {

        }

    

        [HttpGet("")] // => Select
        // [HttpPost] => INSERT
        // [HttpPut] => Update complet
        //[HttpPatch] => UpdateAdapter Partiel
        // [HttpDelete] => Delete
        public async Task<ActionResult<IEnumerable<IFilm>>> SearchFilm(
            FilmSearch search,
            [FromServices] IAlloCineService service,
            [FromServices] IServiceProvider injector
            )
        {
            if (!this.ModelState.IsValid)
            {
                // retour d'érreur de requete => indique à l'utilisateur pourquoi le Cinema n'était pas correct
                return BadRequest(this.ModelState);
            }

            // Si ce service n'est utile que dans certains cas d'exécution
            // Je le demande que lorsque nécessaire
            var serviceIci = injector.GetRequiredService<IAlloCineService>();
            var resultats=await service.SearchFilmsAsync(search);
            return Ok(resultats);
        }
    }
}
