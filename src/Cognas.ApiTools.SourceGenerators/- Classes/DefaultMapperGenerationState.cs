using System;
using System.Collections.Generic;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators;

/// <summary>
///
/// </summary>
public sealed class DefaultMapperGenerationState
{
    #region Field Declarations

    private readonly List<string> _modelNames = [];

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    public DefaultMapperGenerationState()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public bool IsGenerated(string modelName) => _modelNames.Contains(modelName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelName"></param>
    public void SetGenerated(string modelName) => _modelNames.Add(modelName);

    #endregion
}