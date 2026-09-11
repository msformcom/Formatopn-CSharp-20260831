using AlloCineInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlloCineWebApi
{
    [Route("Categorie")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly IAlloCineService service;

        public CategorieController(IAlloCineService service)
        {
            this.service = service;
        }

       

        // GET : /Categorie/GetByFilm/IUAAY171
        [HttpGet("GetByFilm/{codeFilm}")] 
        public async Task<ActionResult<ICategorie>> GetByFilm( string codeFilm)
        {

            var categorie = await service.GetCategoryByFilmAsync(codeFilm);
            return Ok(categorie);
        }
    }
}
