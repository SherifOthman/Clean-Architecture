
using GloboTicket.TicketManagement.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace GloboTicket.TicketManagement.Api.Middleware;

public class ExceptionHandlerMiddleware 
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await ConvertException(context, ex);
        }
    }

    private Task ConvertException(HttpContext context, Exception exception)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        string result = string.Empty;
        
        switch (exception)
        {
            case ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.ValidationErrors);
                break;

            case BadRequestException badRequestException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = badRequestException.Message;
                break;

            case NotFoundException notFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                result = notFoundException.Message;
                break;

            case Exception ex:
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
        }

        context.Response.StatusCode =(int) httpStatusCode;

        if(result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { Message = exception.Message });

        }

        return context.Response.WriteAsync(result);
    }
}
