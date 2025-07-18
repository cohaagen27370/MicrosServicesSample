using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gateway.Filters;

public class ExceptionFilter(IHostEnvironment hostEnvironment, ILogger<ExceptionFilter> logger)
    : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, "An error occurred.");

        if (context.Exception is RpcException exception)
        {
            context.Result = new ObjectResult(new
            {
                StatusCode = 503,
                Message = "Erreur lors de l'appel gRPC.",
                Details = exception.Status.Detail,
                ExceptionType = exception.GetType().Name,
                TraceId = context.HttpContext.TraceIdentifier 
            })
            {
                StatusCode = 500
            };
        }
        else
        {
            var errorResponse = new
            {
                StatusCode = 500,
                Message = "Internal server error.",
                Details = hostEnvironment.IsDevelopment() ? context.Exception.StackTrace : null,
                ExceptionType = hostEnvironment.IsDevelopment() ? context.Exception.GetType().Name : null,
                TraceId = context.HttpContext.TraceIdentifier
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500
            };
        }

        context.ExceptionHandled = true;
    }
}