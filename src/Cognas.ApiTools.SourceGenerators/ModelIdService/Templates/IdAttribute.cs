using System;

namespace Cognas.ApiTools.SourceGenerators.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class IdAttribute : Attribute
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="IdAttribute"/>
    /// </summary>
    public IdAttribute()
    {
    }

    #endregion
}