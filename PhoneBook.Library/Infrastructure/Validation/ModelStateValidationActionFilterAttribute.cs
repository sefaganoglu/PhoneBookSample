using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PhoneBook.Library.Infrastructure.Validation
{
    public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
                context.Result = new ContentResult()
                {
                    Content = "Model state is not valid.'",
                    StatusCode = 400
                };

            base.OnActionExecuting(context);
        }
    }
}
