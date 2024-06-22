namespace Cognas.ApiTools.SourceGenerators.CommandScaffold;

/// <summary>
/// 
/// </summary>
internal readonly record struct CommandScaffoldDetail
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
    public readonly string RequestName;

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
    public readonly bool UseMessaging;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandScaffoldDetail"/>
    /// </summary>
    /// <param name="modelNamespace"></param>
    /// <param name="modelName"></param>
    /// <param name="requestName"></param>
    /// <param name="responseName"></param>
    /// <param name="apiVersion"></param>
    /// <param name="useMessaging"></param>
    public CommandScaffoldDetail(string modelNamespace,
                                 string modelName,
                                 string requestName,
                                 string responseName,
                                 int apiVersion,
                                 bool useMessaging)
    {
        ModelNamespace = modelNamespace;
        ModelName = modelName;
        RequestName = requestName;
        ResponseName = responseName;
        ApiVersion = apiVersion;
        UseMessaging = useMessaging;
    }

    #endregion
}