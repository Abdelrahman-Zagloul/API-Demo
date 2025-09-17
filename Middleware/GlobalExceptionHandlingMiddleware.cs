namespace API_Demo.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Something went wrong.");

                context.Response.StatusCode = 500;
                var errorResponse = new
                {
                    StatusCode = 500,
                    Message = "Internal Server Error",
                    Details = ex.Message
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
