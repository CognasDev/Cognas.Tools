using Microsoft.Extensions.Caching.Memory;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
public interface ICacheBusinessLogic
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    IMemoryCache? MemoryCache { get; }

    /// <summary>
    /// 
    /// </summary>
    string CacheKey { get; }

    /// <summary>
    /// 
    /// </summary>
    int CacheTimeOutMinutes { get; }

    /// <summary>
    /// 
    /// </summary>
    bool UseCache { get; }

    #endregion

    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    Task ResetCacheAsync();

    #endregion
}