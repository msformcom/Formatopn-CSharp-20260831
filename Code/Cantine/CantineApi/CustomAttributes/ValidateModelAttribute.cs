using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CantineApi.CustomAttributes
{
    
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // For API Controllers: Return 400 Bad Request with model state errors
                context.Result = new BadRequestObjectResult(context.ModelState);

                // Alternative for MVC Views: Re-render the view with current model
                // var controller = (Controller)context.Controller;
                // context.Result = controller.View(context.ActionArguments.Values.FirstOrDefault());
            }
        }
    }
}
