using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CantineInterfaces;
using CantineServiceFromBDD.Models;
using HRDAL.DAO;

namespace CantineServiceFromBDD.Mappings
{
    public static class EmployeMapping
    {
        public static void AddEmployeMapping(this IMapperConfigurationExpression config) {
            config.CreateMap<IEmploye, EmployeDAO>() // Mapping auto des propréiétés de meme nom
                         .ForMember(c => c.PublicId, o => o.MapFrom(c => c.Matricule)) // Inutile ca même nom
                          .ForMember(c => c.Surname, o => o.MapFrom(c => c.Prenom))
                           .ForMember(c => c.Name, o => o.MapFrom(c => c.Nom))
                          .ForMember(c => c.BirthDate, o => o.MapFrom(c => c.DateNaissance))
                           .ReverseMap()
                           // Construction de IArticle en utilisant l'objet renvoyé par la fonction fléchée
                           .ConstructUsing(c =>
                           new Employe() { }
                           );
                         
        }
    }
}
