using BeniceSoft.Abp.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;

namespace BeniceSoft.OpenAuthing.Exceptions;

public class AuthingBizException : Exception, IKnownException, IHasHttpStatusCode, IHasLogLevel
{
    public AuthingBizException(int errorCode, string? message, object? errorData = null) : base(message)
    {
        ErrorCode = errorCode;
        ErrorData = errorData;
    }

    public int ErrorCode { get; }
    public object? ErrorData { get; }
    public int HttpStatusCode => (int)System.Net.HttpStatusCode.OK;
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
}