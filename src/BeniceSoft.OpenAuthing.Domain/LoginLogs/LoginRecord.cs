using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.LoginLogs;

/// <summary>
/// 登录日志
/// </summary>
public class LoginLog : AggregateRoot<Guid>
{
    /// <summary>
    /// 用户id
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime LoginTime { get; private set; }

    /// <summary>
    /// IP
    /// </summary>
    public string? Ip { get; private set; }

    /// <summary>
    /// 平台
    /// </summary>
    public string? Platform { get; private set; }

    /// <summary>
    /// 设备
    /// </summary>
    public string? Device { get; private set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string? Browser { get; private set; }

    /// <summary>
    /// 浏览器引擎
    /// </summary>
    public string? Engine { get; private set; }

    /// <summary>
    /// 用户代理
    /// </summary>
    public string? UserAgent { get; private set; }

    private LoginLog(Guid id) : base(id)
    {
    }

    public LoginLog(Guid id, Guid userId, DateTime loginTime, string? ip, string? platform, string? device, string? browser, string? engine,
        string? userAgent)
        : this(id)
    {
        UserId = userId;
        LoginTime = loginTime;
        Ip = ip;
        Platform = platform;
        Device = device;
        Browser = browser;
        Engine = engine;
        UserAgent = userAgent;
    }
}