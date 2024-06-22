using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Builder;

namespace Cognas.ApiTools.MinimalApi;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryApi<TModel, TResponse> where TModel : class where TResponse : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    string PluralModelName { get; }

    /// <summary>
    /// 
    /// </summary>
    string LowerPluralModelName { get; }

    /// <summary>
    /// 
    /// </summary>
    IQueryMappingService<TModel, TResponse> MappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    IPaginationFunctions PaginationFunctions { get; }

    /// <summary>
    /// 
    /// </summary>
    IQueryBusinessLogic<TModel> QueryBusinessLogic { get; }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    void MapAll(WebApplication webApplication);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    void MapGet(WebApplication webApplication);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    void MapGetById(WebApplication webApplication);

    #endregion
}