using System;

namespace Cognas.ApiTools.SourceGenerators.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class IncludeInModelIdServiceAttribute : Attribute
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="IncludeInModelIdServiceAttribute"/>
    /// </summary>
    public IncludeInModelIdServiceAttribute()
    {
    }

    #endregion
}