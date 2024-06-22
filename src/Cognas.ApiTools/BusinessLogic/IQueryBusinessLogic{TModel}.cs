using Cognas.ApiTools.Data.Query;
using Cognas.ApiTools.Shared;
using Microsoft.Extensions.Caching.Memory;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IQueryBusinessLogic<TModel> : ICommandOrQueryBusinessLogic where TModel : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    IQueryDatabaseService DatabaseService { get; }

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

    /// <summary>
    /// 
    /// </summary>
    string SelectStoredProcedure { get; }

    /// <summary>
    /// 
    /// </summary>
    string SelectByIdStoredProcedure { get; }

    #endregion

    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TModel>> SelectModelsAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idExpression"></param>
    /// <returns></returns>
    Task<TModel?> SelectModelAsync(int id, IParameter idExpression);

    /// <summary>
    /// 
    /// </summary>
    Task ResetCacheAsync();

    #endregion
}