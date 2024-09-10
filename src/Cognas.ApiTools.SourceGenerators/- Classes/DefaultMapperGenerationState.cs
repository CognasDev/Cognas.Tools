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

    private readonly Dictionary<string, string> _names = [];

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
    /// <param name="requestOrResponseName"></param>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public bool IsGenerated(string requestOrResponseName, string modelName) => _names.ContainsKey(requestOrResponseName) && _names[requestOrResponseName] == modelName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestOrResponseName"></param>
    /// <param name="modelName"></param>
    public void SetGenerated(string requestOrResponseName, string modelName) => _names.Add(requestOrResponseName, modelName);

    #endregion
}