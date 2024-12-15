using RockPaperScissorsGame.Core.Exceptions;
using System.Net;
using Newtonsoft.Json;

namespace RockPaperScissorsGame.Api.Middlewares
{
    /// <summary>
    /// Middleware to handle exceptions globally and provide consistent error responses.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        /// <summary>
        /// The next middleware in the request pipeline.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware to invoke in the pipeline.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to process the HTTP request and catch exceptions.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception and generates a JSON response with appropriate status code.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="ex">The exception that was thrown.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Determine the status code based on the exception type
            var statusCode = ex switch
            {
                GameNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                InvalidMoveException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            // Create the response object
            var response = new { message = ex.Message };

            // Set response headers and content type
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Serialize the response object to JSON and write it to the response
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
