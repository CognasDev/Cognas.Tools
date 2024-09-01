using System;

namespace Cognas.ApiTools.SourceGenerators.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class QueryScaffoldAttribute : Attribute
{
    #region Property Declarations

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
    public bool UseDefaultMapper { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryScaffoldAttribute"/>
    /// </summary>
    /// <param name="responseType"></param>
    /// <param name="apiVersion"></param>
    /// <param name="useDefaultMapper"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public QueryScaffoldAttribute(Type responseType, int apiVersion, bool useDefaultMapper)
    {
        if (apiVersion < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(apiVersion));
        }
        ResponseType = responseType;
        ApiVersion = apiVersion;
        UseDefaultMapper = useDefaultMapper;
    }

    #endregion
}