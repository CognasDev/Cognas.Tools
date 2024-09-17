using Cognas.ApiTools.ServiceRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Cognas.ApiTools.Messaging;

/// <summary>
/// 
/// </summary>
public static class SignalRExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddSignalRServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSignalR();
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IModelMessagingService<>), ServiceLifetime.Singleton);
    }

    #endregion
}