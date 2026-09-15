using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CantineServiceFromBDD.Models;
using HRDAL.DAO;

namespace CantineServiceFromBDD
{
    // Regroupe les règles de conversion DAO (couche BDD) vers Models (couche métier)
    // Alternative au Profile : des fonctions static qui alimentent l'IMapperConfigurationExpression
    // Les noms des propriétés diffèrent des deux côtés : chaque membre est donc explicite
    public static class CantineMapping
    {
        // Point d'entrée unique, passé à AddAutoMapper lors de l'enregistrement dans la DI
        public static void Configure(IMapperConfigurationExpression config)
        {
            ConfigureArticle(config);
            ConfigureEmploye(config);
        }

        public static void ConfigureArticle(IMapperConfigurationExpression config)
        {
            config.CreateMap<ArticleDAO, Article>()
                .ForMember(dest => dest.Libelle, options => options.MapFrom(src => src.Label))
                .ForMember(dest => dest.Prix, options => options.MapFrom(src => src.Price))
                // Allergens est une chaine séparée par des , dans la BDD, null si non renseigné
                .ForMember(dest => dest.Allergenes, options => options.MapFrom(
                    src => src.Allergens == null
                        ? new List<string>()
                        : src.Allergens.Split(',').ToList()));
        }

        public static void ConfigureEmploye(IMapperConfigurationExpression config)
        {
            config.CreateMap<EmployeDAO, Employe>()
                .ForMember(dest => dest.Matricule, options => options.MapFrom(src => src.PublicId))
                .ForMember(dest => dest.DateNaissance, options => options.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Nom, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.Prenom, options => options.MapFrom(src => src.Surname));
        }
    }
}
