using System.Collections.Generic;

namespace Cognas.ApiTools.SourceGenerators.QueryScaffold;

/// <summary>
/// 
/// </summary>
internal readonly record struct QueryScaffoldDetail
{
    #region Field Declarations

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
    public readonly string ResponseName;

    /// <summary>
    /// 
    /// </summary>
    public readonly int ApiVersion;

    /// <summary>
    /// 
    /// </summary>
    public readonly bool UseDefaultMapper;

    /// <summary>
    /// 
    /// </summary>
    public readonly string IdPropertyName;

    /// <summary>
    /// 
    /// </summary>
    public readonly IEnumerable<string> PropertyNames;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryScaffoldDetail"/>
    /// </summary>
    /// <param name="modelNamespace"></param>
    /// <param name="modelName"></param>
    /// <param name="responseName"></param>
    /// <param name="apiVersion"></param>
    /// <param name="useDefaultMapper"></param>
    /// <param name="idPropertyName"></param>
    /// <param name="propertyNames"></param>
    public QueryScaffoldDetail(string modelNamespace,
                               string modelName,
                               string responseName,
                               int apiVersion,
                               bool useDefaultMapper,
                               string idPropertyName,
                               IEnumerable<string> propertyNames)
    {
        ModelNamespace = modelNamespace;
        ModelName = modelName;
        ResponseName = responseName;
        ApiVersion = apiVersion;
        UseDefaultMapper = useDefaultMapper;
        IdPropertyName = idPropertyName;
        PropertyNames = propertyNames;
    }

    #endregion
}