using AutoMapper;
using CantineServiceFromBDD.Mappings;

namespace CantineServiceFromBDD
{
    // Point d'entrée unique du mapping, passé à AddAutoMapper lors de l'enregistrement dans la DI
    // Les règles vivent dans Mappings, une fonction static par entité
    public static class CantineMapping
    {
        public static void Configure(IMapperConfigurationExpression config)
        {
            config.AddArticleMapping();
            config.AddEmployeMapping();
        }
    }
}
