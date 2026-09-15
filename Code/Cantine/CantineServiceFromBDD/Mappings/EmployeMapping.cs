using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CantineServiceFromBDD.Models;
using HRDAL.DAO;

namespace CantineServiceFromBDD.Mappings
{
    public static class EmployeMapping
    {
        public static void AddEmployeMapping(this IMapperConfigurationExpression config) {

            // Lecture : la BDD vers le métier
            config.CreateMap<EmployeDAO, Employe>()
                         .ForMember(c => c.Matricule, o => o.MapFrom(c => c.PublicId))
                          .ForMember(c => c.DateNaissance, o => o.MapFrom(c => c.BirthDate))
                           .ForMember(c => c.Nom, o => o.MapFrom(c => c.Name))
                            .ForMember(c => c.Prenom, o => o.MapFrom(c => c.Surname));
        }
    }
}
