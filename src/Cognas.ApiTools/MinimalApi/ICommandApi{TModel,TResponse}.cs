using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

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
    /// <param name="endpointRouteBuilder"></param>
    void MapAll(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapPost(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapPut(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapDelete(IEndpointRouteBuilder endpointRouteBuilder);

    #endregion
}