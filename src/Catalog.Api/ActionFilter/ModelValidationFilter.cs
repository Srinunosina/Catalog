using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.Api.ActionFilter;
public class ModelValidationFilter : IActionFilter
{
    /// <summary>
    /// If the model state is invalid, return a 400 Bad Request with the validation errors.
    /// if controller are not decorated with [ApiController], model validation does not happen automatically.
    /// So we need to do it manually here. throwing validation errors in a consistent format using action filter.
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(
                    k => k.Key,
                    v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new { Errors = errors });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
