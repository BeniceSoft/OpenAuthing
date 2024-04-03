using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.Users;

/// <summary>
/// 用户
/// </summary>
public sealed class User : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; protected internal set; }

    /// <summary>
    /// 归一化用户名
    /// </summary>
    public string NormalizedUserName { get; protected internal set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; private set; }

    /// <summary>
    /// 哈希后的密码
    /// </summary>
    public string PasswordHash { get; protected internal set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? PhoneNumber { get; protected internal set; }

    /// <summary>
    /// 手机号码是否确认
    /// </summary>
    public bool PhoneNumberConfirmed { get; protected internal set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; private set; }

    /// <summary>
    /// 性别
    /// </summary>
    public string? Gender { get; private set; }

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; private set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; private set; }

    /// <summary>
    /// 锁定结束时间
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; protected internal set; }

    /// <summary>
    /// 锁定状态
    /// </summary>
    public bool LockoutEnabled { get; protected internal set; }

    /// <summary>
    /// 错误次数
    /// </summary>
    public int AccessFailedCount { get; protected internal set; }

    /// <summary>
    /// 安全凭证
    /// </summary>
    public string SecurityStamp { get; protected internal set; }

    /// <summary>
    /// 是否启用 2FA
    /// </summary>
    public bool TwoFactorEnabled { get; protected internal set; }

    /// <summary>
    /// 是否系统内置
    /// </summary>
    public bool IsSystemBuiltIn { get; private set; }

    /// <summary>
    /// 第三方登录信息
    /// </summary>
    public IReadOnlyCollection<UserLogin> Logins => _logins;

    private readonly List<UserLogin> _logins;

    public IReadOnlyCollection<UserToken> Tokens => _tokens;

    private readonly List<UserToken> _tokens;

    private User(Guid id) : base(id)
    {
        _tokens = new();
        _logins = new();
    }

    public User(Guid id, string userName, string phoneNumber, bool phoneNumberConfirmed)
        : this(id)
    {
        UserName = userName;
        Nickname = userName;
        PhoneNumber = phoneNumber;
        Enabled = true;
        PhoneNumberConfirmed = phoneNumberConfirmed;
    }

    public User(Guid id, string userName, string nickname, string? phoneNumber = null, string? avatar = null, string? gender = null,
        string? jobTitle = null, bool isSystemBuiltIn = false)
        : this(id)
    {
        Check.NotNullOrWhiteSpace(userName, nameof(userName));

        UserName = userName;
        Nickname = nickname;
        PhoneNumber = phoneNumber;
        Avatar = avatar;
        Gender = gender;
        JobTitle = jobTitle;
        Enabled = true;
        IsSystemBuiltIn = isSystemBuiltIn;
    }

    public void SetNormalizedUserName(string normalizedUserName)
    {
        NormalizedUserName = normalizedUserName;
    }

    public void Update(string userName, string nickname, string phoneNumber, string avatar, string gender, string jobTitle)
    {
        UserName = userName;
        Nickname = nickname;
        PhoneNumber = phoneNumber;
        Avatar = avatar;
        Gender = gender;
        JobTitle = jobTitle;
        //UpdateRoles(roleIds.ToArray());,List<Guid> roleIds
    }

    public void AddLogin(UserLoginInfo login)
    {
        if (_logins.Any(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey))
        {
            return;
        }

        _logins.Add(new(Id, login));
    }

    public void RemoveLogin(string loginProvider, string providerKey)
    {
        _logins.RemoveAll(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
    }

    public void ChangeIsEnabled(bool? isEnabled = null)
    {
        if (!isEnabled.HasValue)
        {
            Enabled = !Enabled;
        }
        else
        {
            Enabled = isEnabled.Value;
        }
    }

    public UserToken? FindToken(string loginProvider, string name)
    {
        return _tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
    }

    public void SetToken(string loginProvider, string name, string value)
    {
        var token = FindToken(loginProvider, name);
        if (token is null)
        {
            _tokens.Add(new UserToken(Id, loginProvider, name, value));
        }
        else
        {
            token.Value = value;
        }
    }

    public void RemoveToken(string loginProvider, string name)
    {
        _tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
    }

    public void UpdateAvatar(string avatar)
    {
        Avatar = avatar;
    }
}