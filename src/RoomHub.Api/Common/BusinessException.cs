namespace RoomHub.Api.Common;

// Business errors are still exceptions at this step, caught by ExceptionMiddleware.
public sealed class BusinessException(string code, string message, int statusCode)
    : Exception(message)
{
    public string Code { get; } = code;
    public int StatusCode { get; } = statusCode;
}
