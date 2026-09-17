using AutoMapper;
using CantineInterfaces;
using CantineServiceFromBDD.Models;
using HRDAL.DAO;

namespace CantineServiceFromBDD.Mappings
{
    public static class EmployeMapping
    {
        public static void AddEmployeMapping(this IMapperConfigurationExpression config) {
            config.CreateMap<IEmploye, EmployeDAO>()
                         .ForMember(c => c.PublicId, o => o.MapFrom(c => c.Matricule))
                          .ForMember(c => c.BirthDate, o => o.MapFrom(c => c.DateNaissance))
                           .ForMember(c => c.Name, o => o.MapFrom(c => c.Nom))
                            .ForMember(c => c.Surname, o => o.MapFrom(c => c.Prenom))
                           .ReverseMap()
                           // Construction de IEmploye : AutoMapper ne sait pas instancier une interface
                           .ConstructUsing(c => new Employe());
        }
    }
}
