using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Builder;

namespace Cognas.ApiTools.MinimalApi;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandApi<TModel, TRequest, TResponse>
    where TModel : class
    where TRequest : class
    where TResponse : class
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
    ICommandMappingService<TModel, TRequest, TResponse> CommandMappingService { get; }

    /// <summary>
    /// 
    /// </summary>
    ICommandBusinessLogic<TModel> CommandBusinessLogic { get; }

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
    void MapPost(WebApplication webApplication);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    void MapPut(WebApplication webApplication);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    void MapDelete(WebApplication webApplication);

    #endregion
}