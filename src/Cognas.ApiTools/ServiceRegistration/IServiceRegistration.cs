using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cognas.ApiTools.ServiceRegistration.Abstractions;

/// <summary>
/// 
/// </summary>
public interface IServiceRegistration
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="interfaceType"></param>
    /// <param name="serviceLifetime"></param>
    /// <param name="assembly"></param>
    void AddServices(IServiceCollection serviceCollection, Type interfaceType, ServiceLifetime serviceLifetime, Assembly? assembly = null);

    #endregion
}