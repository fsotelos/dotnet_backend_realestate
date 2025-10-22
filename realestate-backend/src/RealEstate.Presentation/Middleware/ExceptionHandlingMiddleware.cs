using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using RealEstate.Domain.Exceptions;
using Serilog;

namespace RealEstate.Presentation.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (DomainException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            var trackErrorId = Guid.NewGuid().ToString();

            _logger.LogError(exception, "Error occurred: {Path} | ID: {trackErrorId}", context.Request.Path, trackErrorId);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                errorId = trackErrorId,
                message = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred. Our team has been notified."
                    : exception.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}