using System.Net;
using Fintacharts.Application.Common.Exceptions;
using Fintacharts.Domain.DTO;
using ILogger = Serilog.ILogger;

namespace Fintacharts.API.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ResolveHttpStatusCode(exception);

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }

        private HttpStatusCode ResolveHttpStatusCode(Exception exception)
        {
            return exception switch
            {
                EntityNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}
