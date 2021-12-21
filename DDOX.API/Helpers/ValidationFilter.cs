using DDOX.API.Dtos.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Helpers
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage)).ToArray();

                var errorResponse = new List<ErrorModel>();
                foreach (var error in errorsInModelState)
                {
                    foreach (var suberror in error.Value)
                    {
                        errorResponse.Add(new ErrorModel
                        {
                            Key = error.Key,
                            Message = suberror
                        });
                    }
                }
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }
}
