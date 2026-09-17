using CantineAPI.Models;
using CantineInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace CantineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchatsController : ControllerBase
    {
        private readonly ICantineService service;

        public AchatsController(ICantineService service)
        {
            this.service = service;
        }

        // POST api/achats
        [HttpPost]
        public async Task<IActionResult> Consommer([FromBody] ConsommationRequest consommation)
        {
            try
            {
                await service.ConsommerArticleAsync(
                    consommation.MatriculeEmploye,
                    consommation.ReferenceArticle,
                    consommation.Quantite);
            }
            catch (ArgumentException ex)
            {
                // Erreurs metier du service : reference inconnue, stock ou credit insuffisant
                // Le message part en text/plain, tel que le lit le client
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
