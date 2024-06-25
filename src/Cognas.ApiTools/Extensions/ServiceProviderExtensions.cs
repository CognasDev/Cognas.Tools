using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceProviderExtensions
{
    #region Static Method Declarations

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
        where TRequest : class
        where TResponse : class
    {
        IEnumerable<ICommandApi<TModel, TRequest, TResponse>> commandApis = serviceProvider.GetServices<ICommandApi<TModel, TRequest, TResponse>>();
        ICommandApi<TModel, TRequest, TResponse> commandApi = commandApis.FastFirstOrDefault(commandApi => commandApi.ApiVersion == apiVersion) ?? throw new NullReferenceException();
        return commandApi;
    }

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
    {
        IEnumerable<IQueryApi<TModel, TResponse>> queryApis = serviceProvider.GetServices<IQueryApi<TModel, TResponse>>();
        IQueryApi<TModel, TResponse> queryApi = queryApis.FastFirstOrDefault(queryApi => queryApi.ApiVersion == apiVersion) ?? throw new NullReferenceException();
        return queryApi;
    }

    #endregion

}