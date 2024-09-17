using Cognas.ApiTools.ServiceRegistration;
using Microsoft.AspNetCore.SignalR;
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
    public static ISignalRServerBuilder AddSignalRServices(this IServiceCollection serviceCollection)
    {
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IModelMessagingService<>), ServiceLifetime.Singleton);
        return serviceCollection.AddSignalR();
    }

    #endregion
}