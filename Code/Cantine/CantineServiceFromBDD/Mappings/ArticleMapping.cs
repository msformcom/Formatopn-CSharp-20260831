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
                           // La collection est initialisée ici, le AfterMap ne fait que la remplir
                           .ConstructUsing(c =>
                           new Article() { Allergenes = new List<string>() }
                           )
                          .AfterMap((src, dest, context) =>
                          {
                              // String.Join n'a pas d'inverse automatique : on redécoupe à la main
                              if (src.Allergens == null) return;
                              foreach (var itemSrc in src.Allergens.Split(","))
                              {
                                  dest.Allergenes.Add(itemSrc);
                              }
                          });
        }
    }
}
