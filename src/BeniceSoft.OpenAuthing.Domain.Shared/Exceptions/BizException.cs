using BeniceSoft.Abp.Core.Exceptions;

namespace BeniceSoft.OpenAuthing.Exceptions;

public class BizException : Exception, IKnownException
{
    public BizException(int errorCode, string? message = null, object? errorData = null) : base(message)
    {
        ErrorCode = errorCode;
        ErrorData = errorData;
    }
    
    public int ErrorCode { get; }
    public object? ErrorData { get; }
}