namespace RockPaperScissorsGame.Api.Middlewares
{
    /// <summary>
    /// Middleware to limit the rate of incoming requests from the same IP address.
    /// </summary>
    public class RateLimitingMiddleware
    {
        /// <summary>
        /// Stores the last request time for each IP address.
        /// </summary>
        private static readonly Dictionary<string, DateTime> _requestTimes = new();

        /// <summary>
        /// Delegate to invoke the next middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="RateLimitingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Processes the incoming HTTP request and applies rate limiting based on the client's IP address.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Get the client's IP address
            var ip = context.Connection.RemoteIpAddress?.ToString();

            // Check if the IP has made a request recently
            if (ip != null && _requestTimes.ContainsKey(ip) && _requestTimes[ip] > DateTime.UtcNow)
            {
                // Rate limit exceeded
                context.Response.StatusCode = 429; // Too Many Requests
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return;
            }

            // Allow the request and update the last request time
            if (ip != null)
            {
                _requestTimes[ip] = DateTime.UtcNow.AddSeconds(5);
            }

            // Proceed to the next middleware
            await _next(context);
        }
    }
}
