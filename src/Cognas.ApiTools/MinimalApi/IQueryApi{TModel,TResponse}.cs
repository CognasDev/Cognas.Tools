using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Routing;

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
    int ApiVersion { get; }

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
    IQueryMappingService<TModel, TResponse> QueryMappingService { get; }

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
    /// <param name="endpointRouteBuilder"></param>
    void MapAll(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapGet(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapGetById(IEndpointRouteBuilder endpointRouteBuilder);

    #endregion
}