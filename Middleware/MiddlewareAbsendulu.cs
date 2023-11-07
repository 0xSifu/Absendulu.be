using System.Net;
using AbsenDulu.BE.Response;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Middleware;
public class MiddlewareAbsendulu
{
    private readonly RequestDelegate _next;
    public MiddlewareAbsendulu(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(TokenException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            ErrorResponse errorResponse = new ErrorResponse
            {
                status = "401 Unauthorized",
                message = exception.Message
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            return;
        }
        catch(IdentityException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            ErrorResponse errorResponse = new ErrorResponse
            {
                status = "403 Forbiden",
                message = exception.Message
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            return;
        }
        catch(ServerException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ErrorResponse errorResponse = new ErrorResponse
            {
                status = "500 Internal Server Error",
                message = exception.Message
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            return;
        }
        catch(CredentialException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            ErrorResponse errorResponse = new ErrorResponse
            {
                status = "403 Forbidden",
                message = exception.Message
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            return;
        }

        catch(ValidationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ErrorResponse errorResponse = new ErrorResponse
            {
                status = "400 Bad Request",
                message = exception.Message
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            return;
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ErrorResponse errorResponse = new ErrorResponse
            {
                status = "500 Internal Server Error",
                message = ex.Message
            };

            if (ex is BadHttpRequestException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.status = "400 Bad Request";
            }
            else if (ex is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.status = "401 Unauthorized";
            }
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            return;
        }
    }
}