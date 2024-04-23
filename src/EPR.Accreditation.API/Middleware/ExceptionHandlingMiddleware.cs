using EPR.Accreditation.API.Helpers;
using System.Diagnostics;

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
                var result = FormatException(string.Empty, ex);

                // Log other exceptions
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
#if DEBUG
                Debug.WriteLine(result);
#endif
            }
        }

        private string FormatException(
            string currentError,
            Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(currentError))
            {
                currentError = FormatSingleError(ex);
            }

            if (ex.InnerException != null)
            {
                var stringPart = FormatException(
                    currentError,
                    ex.InnerException);

                if (!string.IsNullOrWhiteSpace(stringPart))
                {
                    return string.Format($"{currentError}{ex.InnerException.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}");
                }
            }

            return currentError;
        }

        private string FormatSingleError(Exception ex)
        {
            return $"{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
        }
    }
}
