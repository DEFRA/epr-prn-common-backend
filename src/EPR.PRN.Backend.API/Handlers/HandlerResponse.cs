#nullable disable
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage]
public class HandlerResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }

    public HandlerResponse(int statusCode, T data = default, string message = null)
    {
        StatusCode = statusCode;
        Data = data;
        Message = message;
    }
}
