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
        public async Task<IEnumerable<IArticle>> Search([FromBody] ArticleSearch search)
        {
            var articles = await service.ListeArticlesAsync(search);
            // Le resultat peut etre un IQueryable : on le materialise avant
            // que le CantineContext de la requete ne soit libere
            return articles.ToList();
        }
    }
}
