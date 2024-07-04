using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.MinimalApi;
using Cognas.Tools.Shared.Extensions;
using System;

namespace Cognas.ApiTools.Endpoints;

/// <summary>
/// Auto-generated to gather all query endpoints for all api versions and initialise them.
/// NB: a separate partial declaration exists for command endpoints.
/// </summary>
#nullable enable
public static partial class EndpointInitiator
{{
    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public static void InitiateQueryEndpoints(this WebApplication webApplication)
    {{
{0}
    }}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="webApplication"></param>
    /// <param name="apiVersion"></param>
    /// <param name="endpointRouteBuilder"></param>
    public static void InitiateApi<TModel, TResponse>(this WebApplication webApplication, int apiVersion, IEndpointRouteBuilder? endpointRouteBuilder = null)
        where TModel : class
        where TResponse : class
    {{
        IQueryApi<TModel, TResponse> queryApi = webApplication.Services.GetQueryApi<TModel, TResponse>(apiVersion);
        queryApi.MapAll(endpointRouteBuilder ?? webApplication);
    }}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="apiVersion"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static IQueryApi<TModel, TResponse> GetQueryApi<TModel, TResponse>(this IServiceProvider serviceProvider, int apiVersion)
        where TModel : class where TResponse : class
    {{
        IEnumerable<IQueryApi<TModel, TResponse>> queryApis = serviceProvider.GetServices<IQueryApi<TModel, TResponse>>();
        IQueryApi<TModel, TResponse> queryApi = queryApis.FastFirstOrDefault(queryApi => queryApi.ApiVersion == apiVersion) ?? throw new NullReferenceException();
        return queryApi;
    }}

    #endregion
}}