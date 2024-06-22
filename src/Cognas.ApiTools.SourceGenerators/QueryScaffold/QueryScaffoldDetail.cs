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

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryScaffoldDetail"/>
    /// </summary>
    /// <param name="modelNamespace"></param>
    /// <param name="modelName"></param>
    /// <param name="responseName"></param>
    /// <param name="apiVersion"></param>
    public QueryScaffoldDetail(string modelNamespace,
                               string modelName,
                               string responseName,
                               int apiVersion)
    {
        ModelNamespace = modelNamespace;
        ModelName = modelName;
        ResponseName = responseName;
        ApiVersion = apiVersion;
    }

    #endregion
}