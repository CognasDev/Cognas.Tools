using System.Collections.Generic;

namespace Cognas.ApiTools.SourceGenerators.CommandScaffold;

/// <summary>
/// 
/// </summary>
internal sealed class ApiVersionRepsitory
{
    #region Field Declarations

    private readonly HashSet<int> _apiVersions = [];

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<int> ApiVersions => _apiVersions;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    public ApiVersionRepsitory()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    public void Clear() => _apiVersions.Clear();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="apiVersion"></param>
    /// <returns></returns>
    public bool TryAdd(int apiVersion) => _apiVersions.Add(apiVersion);

    #endregion
}