using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage]
public class HandlerResponse<T>(int statusCode, T data = default, string message = null)
{
    public int StatusCode { get; set; } = statusCode;
    public T Data { get; set; } = data;
    public string Message { get; set; } = message;
}