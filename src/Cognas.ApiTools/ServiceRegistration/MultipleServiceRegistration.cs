using Cognas.ApiTools.ServiceRegistration.Abstractions;
using Cognas.ApiTools.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cognas.ApiTools.ServiceRegistration;

/// <summary>
/// 
/// </summary>
public sealed class MultipleServiceRegistration : ServiceRegistrationBase
{
    #region Field Declarations

    private static readonly Lazy<IServiceRegistration> _lazyInstance = new(() => new MultipleServiceRegistration());

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public static IServiceRegistration Instance => _lazyInstance.Value;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MultipleServiceRegistration"/>
    /// </summary>
    private MultipleServiceRegistration()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="interfaceType"></param>
    /// <param name="serviceLifetime"></param>
    /// <param name="assembly"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void AddServices(IServiceCollection serviceCollection, Type interfaceType, ServiceLifetime serviceLifetime, Assembly? assembly = null)
    {
        string interfaceName = interfaceType.Name;
        GetNonAbstractTypes().FastForEach(type =>
        {
            Type? implementedInterfaceType = type.GetInterface(interfaceName);
            if (implementedInterfaceType != null)
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        serviceCollection.AddSingleton(implementedInterfaceType, type);
                        break;
                    case ServiceLifetime.Scoped:
                        serviceCollection.AddScoped(implementedInterfaceType, type);
                        break;
                    case ServiceLifetime.Transient:
                        serviceCollection.AddTransient(implementedInterfaceType, type);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(Enum.GetName(serviceLifetime));
                }
            }
        });
    }

    #endregion
}