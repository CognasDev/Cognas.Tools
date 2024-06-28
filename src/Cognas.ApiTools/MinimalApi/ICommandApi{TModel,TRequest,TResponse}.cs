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
    where TRequest : notnull
    where TResponse : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    int ApiVersion { get; }

    #endregion

    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    void MapAll(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    RouteHandlerBuilder MapPost(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    RouteHandlerBuilder MapPut(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    RouteHandlerBuilder MapDelete(IEndpointRouteBuilder endpointRouteBuilder);

    #endregion
}