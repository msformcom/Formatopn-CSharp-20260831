using System;
using System.Collections.Generic;
using System.Text;

namespace CantineInterfaces.Tests.Models
{
    public class ArticleSearch : IArticleSearch
    {
        public string? SearchText { get ; set ; }
        public decimal? PrixMax { get ; set ; }
    }
}
