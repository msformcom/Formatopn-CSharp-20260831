using CantineApiContracts;
using CantineAPI.Models;
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
        public async Task<ResponseWrapper<EmployeDto>> Lire(string matricule)
        {
            var employe = await service.LireEmployeInfosAsync(matricule);

            return new ResponseWrapper<EmployeDto>() { Success = true, Data = EmployeDto.Depuis(employe) };
        }
    }
}
