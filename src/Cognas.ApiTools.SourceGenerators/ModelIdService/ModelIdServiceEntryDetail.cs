using System.Collections.Generic;
using System.Text.Json;

namespace Cognas.ApiTools.SourceGenerators.ModelIdService;

/// <summary>
/// 
/// </summary>
internal readonly record struct ModelIdServiceEntryDetail
{
    #region Field Declarations

    /// <summary>
    /// 
    /// </summary>
    public readonly string CamelCaseModelName;

    /// <summary>
    /// 
    /// </summary>
    public readonly string ModelNamespace;

    /// <summary>
    /// 
    /// </summary>
    public readonly string ModelName;

    /// <summary>
    /// 
    /// </summary>
    public readonly string IdPropertyName;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ModelIdServiceEntryDetail"/>
    /// </summary>
    /// <param name="modelNamespace"></param>
    /// <param name="modelName"></param>
    /// <param name="idPropertyName"></param>
    public ModelIdServiceEntryDetail(string modelNamespace, string modelName, string idPropertyName)
    {
        CamelCaseModelName = JsonNamingPolicy.CamelCase.ConvertName(modelName);
        ModelNamespace = modelNamespace;
        ModelName = modelName;
        IdPropertyName = idPropertyName;
    }

    #endregion
}