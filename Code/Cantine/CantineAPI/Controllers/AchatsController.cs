using CantineApiContracts;
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
        public async Task<ActionResult<ResponseWrapper>> Consommer([FromBody] ConsommationRequest consommation)
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
                // Le message voyage dans l'enveloppe, le code HTTP reste un 400
                return BadRequest(new ResponseWrapper() { Success = false, Message = ex.Message });
            }

            return Ok(new ResponseWrapper() { Success = true });
        }
    }
}
