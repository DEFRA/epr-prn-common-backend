using EPR.Accreditation.API.Helpers;

namespace EPR.Accreditation.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException notFound)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain"; // Set the content type as needed
                await context.Response.WriteAsync(notFound.Message); // Include the exception message in the response body
            }
            // Handle other exceptions if needed
            catch (Exception ex)
            {
                // Log other exceptions
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
