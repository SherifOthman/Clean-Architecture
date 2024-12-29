namespace GloboTicket.TicketManagement.Api.Middleware;

public static class MiddlewareExtentions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
       return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
