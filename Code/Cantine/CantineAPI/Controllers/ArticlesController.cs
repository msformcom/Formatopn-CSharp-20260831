using CantineApiContracts;
using CantineAPI.Models;
using CantineInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace CantineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ICantineService service;

        public ArticlesController(ICantineService service)
        {
            this.service = service;
        }

        // POST api/articles/search
        [HttpPost("search")]
        public async Task<ResponseWrapper<IEnumerable<ArticleDto>>> Search([FromBody] ArticleSearch search)
        {
            var articles = await service.ListeArticlesAsync(search);

            // Pagination cote API : le service et la couche d'acces aux donnees
            // restent inchanges
            // La projection en DTO vient apres la pagination : seule la page est mappee
            var page = articles
                .Skip((search.Page - 1) * search.TaillePage)
                .Take(search.TaillePage)
                .Select(ArticleDto.Depuis)
                .ToList();

            return new ResponseWrapper<IEnumerable<ArticleDto>>() { Success = true, Data = page };
        }
    }
}
