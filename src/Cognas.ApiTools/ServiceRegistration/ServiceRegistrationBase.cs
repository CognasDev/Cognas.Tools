using Cognas.ApiTools.ServiceRegistration.Abstractions;
using Cognas.ApiTools.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cognas.ApiTools.ServiceRegistration;

/// <summary>
/// 
/// </summary>
public abstract class ServiceRegistrationBase : IServiceRegistration
{
    #region Field Declarations

    private static readonly Lazy<IEnumerable<Type>> _typesFromEntryAssembly = new(() => GetNonAbstractTypesInternal());

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ServiceRegistrationBase"/>
    /// </summary>
    protected ServiceRegistrationBase()
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
    public abstract void AddServices(IServiceCollection serviceCollection, Type interfaceType, ServiceLifetime serviceLifetime, Assembly? assembly = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    protected static IEnumerable<Type> GetNonAbstractTypes(Assembly? assembly = null) => assembly == null ? _typesFromEntryAssembly.Value : GetNonAbstractTypesInternal(assembly);

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private static List<Type> GetNonAbstractTypesInternal(Assembly? assembly = null)
    {
        List<Type> types = [];
        Assembly assemblyForTypes = assembly ?? Assembly.GetEntryAssembly()!;
        assemblyForTypes.GetTypes().FastForEach(type => !type.IsAbstract, type => types.Add(type));
        return types;
    }

    #endregion
}