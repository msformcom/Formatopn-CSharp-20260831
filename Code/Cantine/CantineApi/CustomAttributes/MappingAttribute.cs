using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CantineApi.CustomAttributes
{
    public class MappingAttribute : ActionFilterAttribute
    {
        private readonly Type tTarget;

        public MappingAttribute(Type TTarget, IMapper mapper)
        {
            tTarget = TTarget;
            Mapper = mapper;
        }

        public IMapper Mapper { get; }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            // modifier ke result pour effectuer le mapping en type TTarget via le mapper 
        }
    }
}
