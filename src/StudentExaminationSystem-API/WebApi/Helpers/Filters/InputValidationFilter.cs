using Application.Helpers;
using FluentValidation;
using WebApi.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Helpers.Filters;

public class InputValidationFilter<TModel> : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Method is not ("POST" or "PUT"))
            return;
        
        var serviceProvider = context.HttpContext.RequestServices;
        var validator = (IValidator<TModel>?)serviceProvider.GetService(typeof(IValidator<TModel>));
        if (validator == null)
        {   
            await next();
            return;
        }

        var model = context.ActionArguments.Values.OfType<TModel>().FirstOrDefault();
        if (model == null)
            return;

        var validationResult = await ValidationHelper.ValidateAndReportAsync<TModel>(validator, model, "Input");
        if (!validationResult.IsSuccess)
        {
            context.Result = validationResult.ToActionResult();
            return;
        }
        await next();
    }
}
