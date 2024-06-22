using System;

namespace Cognas.ApiTools.SourceGenerators.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
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

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryScaffoldAttribute"/>
    /// </summary>
    /// <param name="responseType"></param>
    /// <param name="apiVersion"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public QueryScaffoldAttribute(Type responseType, int apiVersion)
    {
        if (apiVersion < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(apiVersion));
        }
        ResponseType = responseType;
        ApiVersion = apiVersion;
    }

    #endregion
}