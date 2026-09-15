using System;
using System.Collections.Generic;
using System.Linq;
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

            // Lecture : la BDD vers le métier
            config.CreateMap<ArticleDAO, Article>()
                         .ForMember(c => c.Libelle, o => o.MapFrom(c => c.Label))
                          .ForMember(c => c.Prix, o => o.MapFrom(c => c.Price))
                          // Allergens est une chaine séparée par des , dans la BDD, null si non renseigné
                          .ForMember(c => c.Allergenes, o => o.MapFrom(
                              c => c.Allergens == null
                                  ? new List<string>()
                                  : c.Allergens.Split(',').ToList()));

            // Ecriture : le métier vers la BDD
            config.CreateMap<IArticle, ArticleDAO>() // Mapping auto des propréiétés de meme nom
                         .ForMember(c => c.Reference, o => o.MapFrom(c => c.Reference)) // Inutile ca même nom
                          .ForMember(c => c.Price, o => o.MapFrom(c => c.Prix))
                           .ForMember(c => c.Label, o => o.MapFrom(c => c.Libelle))
                          .ForMember(c => c.Allergens, o => o.MapFrom(c => String.Join(",", c.Allergenes)));
        }
    }
}
