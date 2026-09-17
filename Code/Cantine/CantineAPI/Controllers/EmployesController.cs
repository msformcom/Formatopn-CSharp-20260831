using CantineInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace CantineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployesController : ControllerBase
    {
        private readonly ICantineService service;

        public EmployesController(ICantineService service)
        {
            this.service = service;
        }

        // GET api/employes/AA001
        [HttpGet("{matricule}")]
        public async Task<IEmploye> Lire(string matricule)
        {
            return await service.LireEmployeInfosAsync(matricule);
        }
    }
}
