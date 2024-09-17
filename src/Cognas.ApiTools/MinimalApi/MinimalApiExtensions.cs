using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.ServiceRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Cognas.ApiTools.MinimalApi;

/// <summary>
/// 
/// </summary>
public static class MinimalApiExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddMinimalApiServices(this IServiceCollection serviceCollection)
    {
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(ICommandApi<,,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(ICommandBusinessLogic<>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(ICommandMappingService<,>), ServiceLifetime.Singleton);

        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IQueryApi<,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IQueryBusinessLogic<>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IQueryMappingService<,>), ServiceLifetime.Singleton);
    }

    #endregion
}