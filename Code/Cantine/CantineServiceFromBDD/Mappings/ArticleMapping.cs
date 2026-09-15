using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CantineInterfaces;
using CantineServiceFromBDD.Models;
using HRDAL.DAO;

namespace CantineServiceFromBDD.Mappings
{
    public static class ArticleMapping
    {
        public static void AddArticleMapping(this IMapperConfigurationExpression config) {
            config.CreateMap<IArticle, ArticleDAO>() // Mapping auto des propréiétés de meme nom
                         .ForMember(c => c.Reference, o => o.MapFrom(c => c.Reference)) // Inutile ca même nom
                          .ForMember(c => c.Price, o => o.MapFrom(c => c.Prix))
                           .ForMember(c => c.Label, o => o.MapFrom(c => c.Libelle))
                          .ForMember(c => c.Allergens, o => o.MapFrom(c => String.Join(",", c.Allergenes)))
                           .ReverseMap()
                           // Construction de IArticle en utilisant l'objet renvoyé par la fonction fléchée
                           .ConstructUsing(c => 
                           new Article() { }
                           )
                          .AfterMap((src, dest, context) =>
                          {
                              foreach (var itemSrc in src.Allergens.Split(","))
                              {
                                  ;
                                  dest.Allergenes.Add(itemSrc);
                              }
                          });
        }
    }
}
