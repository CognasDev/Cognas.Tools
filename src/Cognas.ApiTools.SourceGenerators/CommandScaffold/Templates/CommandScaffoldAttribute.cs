using System;

namespace Cognas.ApiTools.SourceGenerators.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CommandScaffoldAttribute : Attribute
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public Type RequestType { get; }

    /// <summary>
    /// 
    /// </summary>
    public Type ResponseType { get; }

    /// <summary>
    /// 
    /// </summary>
    public int ApiVersion { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool UseMessaging { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool UseDefaultMapper { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandScaffoldAttribute"/>
    /// </summary>
    /// <param name="requestType"></param>
    /// <param name="responseType"></param>
    /// <param name="apiVersion"></param>
    /// <param name="useMessaging"></param>
    /// <param name="useDefaultMapper"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public CommandScaffoldAttribute(Type requestType, Type responseType, int apiVersion, bool useMessaging, bool useDefaultMapper)
    {
        if (apiVersion < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(apiVersion));
        }
        RequestType = requestType;
        ResponseType = responseType;
        ApiVersion = apiVersion;
        UseMessaging = useMessaging;
        UseDefaultMapper = useDefaultMapper;
    }

    #endregion
}