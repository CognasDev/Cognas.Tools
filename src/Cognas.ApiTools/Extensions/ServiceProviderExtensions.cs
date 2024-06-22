using Cognas.ApiTools.MinimalApi;
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
    /// <exception cref="NullReferenceException"></exception>
    public static ICommandApi<TModel, TRequest, TResponse> GetCommandApi<TModel, TRequest, TResponse>(this IServiceProvider serviceProvider)
        where TModel : class
        where TRequest : class
        where TResponse : class
    {
        ICommandApi<TModel, TRequest, TResponse> minimalApiMap = serviceProvider.GetService<ICommandApi<TModel, TRequest, TResponse>>() ?? throw new NullReferenceException();
        return minimalApiMap;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <exception cref="NullReferenceException"></exception>
    public static IQueryApi<TModel, TResponse> GetQueryApi<TModel, TResponse>(this IServiceProvider serviceProvider)
        where TModel : class where TResponse : class
    {
        IQueryApi<TModel, TResponse> minimalApiMap = serviceProvider.GetService<IQueryApi<TModel, TResponse>>() ?? throw new NullReferenceException();
        return minimalApiMap;
    }

    #endregion

}