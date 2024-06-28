using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.Shared.Extensions;
using System;

namespace Cognas.ApiTools.Endpoints;

/// <summary>
/// Auto-generated to gather all command endpoints for all api versions and initialise them.
/// NB: a separate partial declaration exists for query endpoints.
/// </summary>
#nullable enable
public static partial class EndpointInitiator
{{
    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public static void InitiateCommandEndpoints(this WebApplication webApplication)
    {{
{0}
    }}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="webApplication"></param>
    /// <param name="apiVersion"></param>
    /// <param name="endpointRouteBuilder"></param>
    public static void InitiateApi<TModel, TRequest, TResponse>(this WebApplication webApplication, int apiVersion, IEndpointRouteBuilder? endpointRouteBuilder = null)
        where TModel : class
        where TRequest : notnull
        where TResponse : class
    {{
        ICommandApi<TModel, TRequest, TResponse> commandApi = webApplication.Services.GetCommandApi<TModel, TRequest, TResponse>(apiVersion);
        commandApi.MapAll(endpointRouteBuilder ?? webApplication);
    }}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="apiVersion"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static ICommandApi<TModel, TRequest, TResponse> GetCommandApi<TModel, TRequest, TResponse>(this IServiceProvider serviceProvider, int apiVersion)
        where TModel : class
        where TRequest : notnull
        where TResponse : class
    {{
        IEnumerable<ICommandApi<TModel, TRequest, TResponse>> commandApis = serviceProvider.GetServices<ICommandApi<TModel, TRequest, TResponse>>();
        ICommandApi<TModel, TRequest, TResponse> commandApi = commandApis.FastFirstOrDefault(commandApi => commandApi.ApiVersion == apiVersion) ?? throw new NullReferenceException();
        return commandApi;
    }}

    #endregion
}}