using Authentication.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authentication.Middleware
{
    public class ErrorHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var error = new ErrorModel()
            {
                Message = context.Exception.Message,
                Status = StatusCodes.Status500InternalServerError
            };

            if (context.Exception is APIError apiError)
            {
                error.Status = apiError.StatusCode;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = error.Status;
            context.Result = new JsonResult(error);
        }
    }
}
