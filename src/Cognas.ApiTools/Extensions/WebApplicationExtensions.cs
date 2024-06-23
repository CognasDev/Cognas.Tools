using Cognas.ApiTools.MinimalApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class WebApplicationExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="webApplication"></param>
    /// <param name="endpointRouteBuilder"></param>
    public static void InitiateApi<TModel, TResponse>(this WebApplication webApplication, IEndpointRouteBuilder? endpointRouteBuilder = null)
        where TModel : class
        where TResponse : class
    {
        IQueryApi<TModel, TResponse> queryApi = webApplication.Services.GetQueryApi<TModel, TResponse>();
        queryApi.MapAll(endpointRouteBuilder ?? webApplication);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="webApplication"></param>
    /// <param name="endpointRouteBuilder"></param>
    public static void InitiateApi<TModel, TRequest, TResponse>(this WebApplication webApplication, IEndpointRouteBuilder? endpointRouteBuilder = null)
        where TModel : class
        where TRequest : class
        where TResponse : class
    {
        ICommandApi<TModel, TRequest, TResponse> commandApi = webApplication.Services.GetCommandApi<TModel, TRequest, TResponse>();
        commandApi.MapAll(endpointRouteBuilder ?? webApplication);
        webApplication.InitiateApi<TModel, TResponse>(endpointRouteBuilder);
    }

    #endregion
}