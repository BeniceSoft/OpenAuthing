using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Volo.Abp;

namespace BeniceSoft.OpenAuthing.DynamicAuth;

/// <summary>
/// TODO 待处理分布式部署的问题
/// </summary>
public class DynamicAuthenticationManager : IDynamicAuthenticationManager
{
    /// <summary>
    /// Gets the type of the managed handler.
    /// </summary>
    /// <value>
    /// The type of the managed handler.
    /// </value>
    public virtual IReadOnlyDictionary<string, Type> ManagedHandlerType { get; }

    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly OAuthOptionsMonitorCacheWrapperFactory _wrapperFactory;

    public DynamicAuthenticationManager(IAuthenticationSchemeProvider schemeProvider,
        OAuthOptionsMonitorCacheWrapperFactory wrapperFactory,
        IReadOnlyDictionary<string, Type> managedHandlerType)
    {
        _schemeProvider = schemeProvider;
        _wrapperFactory = wrapperFactory;
        ManagedHandlerType = managedHandlerType;
    }


    public virtual void Add(string schemeName, string name, string displayName, IReadOnlyDictionary<string, string> optionsDictionary)
    {
        var handlerType = ManagedHandlerType.GetValueOrDefault(schemeName);
        if (handlerType is null)
        {
            throw new UserFriendlyException($"未知的处理类型：{schemeName}");
        }

        var optionsType = GetOptionsType(handlerType);
        var options = ConstructTargetOAuthOptions(optionsType, optionsDictionary);

        var optionsMonitorCache = _wrapperFactory.Get(optionsType);

        _schemeProvider.AddScheme(new AuthenticationScheme(name, displayName, handlerType));
        optionsMonitorCache.TryAdd(name, options);
    }

    public virtual void Remove(string schemeName, string name)
    {
        var handlerType = ManagedHandlerType.GetValueOrDefault(schemeName);
        if (handlerType is null)
        {
            throw new UserFriendlyException($"未知的处理类型：{schemeName}");
        }

        var optionsType = GetOptionsType(handlerType);

        var optionsMonitorCache = _wrapperFactory.Get(optionsType);

        _schemeProvider.RemoveScheme(name);
        optionsMonitorCache.TryRemove(name);
    }

    private static AuthenticationSchemeOptions ConstructTargetOAuthOptions(Type optionsType, IReadOnlyDictionary<string, string> optionsDictionary)
    {
        var options = (AuthenticationSchemeOptions)Activator.CreateInstance(optionsType)!;
        var propertyInfos = GetOptionsProperties(optionsType);
        foreach (var propertyInfo in propertyInfos)
        {
            var stringValue = optionsDictionary.GetValueOrDefault(propertyInfo.Name);
            if (string.IsNullOrWhiteSpace(stringValue)) continue;

            object? convertedValue = null;
            try
            {
                convertedValue = Convert.ChangeType(stringValue, propertyInfo.PropertyType);
            }
            catch (Exception e)
            {
                // 转换失败

                // TODO 尝试使用 JSON 反序列化
            }

            propertyInfo.SetValue(options, convertedValue);
        }

        options.Validate();
        return options!;
    }

    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> CachedOptionsPropertyInfosMap = new();

    private static PropertyInfo[] GetOptionsProperties(Type optionsType)
    {
        return CachedOptionsPropertyInfosMap.GetOrAdd(optionsType, type => type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty));
    }

    private static Type GetOptionsType(Type handlerType)
    {
        if (!typeof(IAuthenticationHandler).IsAssignableFrom(handlerType))
        {
            throw new ArgumentException($"Parameter {nameof(handlerType)} should be a {nameof(AuthenticationHandler<AuthenticationSchemeOptions>)}");
        }

        var genericTypeArguments = GetGenericTypeArguments(handlerType);
        var optionsType = genericTypeArguments[0];
        return optionsType ?? throw new UserFriendlyException("没有找到处理类型对应的配置类");
    }

    private static Type[] GetGenericTypeArguments(Type handlerType)
    {
        if (handlerType.GenericTypeArguments.Length == 1)
        {
            return handlerType.GenericTypeArguments;
        }

        if (handlerType.BaseType == null || handlerType.BaseType == typeof(object))
        {
            throw new ArgumentException($"Parameter {nameof(handlerType)} should be a {nameof(AuthenticationHandler<AuthenticationSchemeOptions>)}");
        }

        return GetGenericTypeArguments(handlerType.BaseType);
    }
}