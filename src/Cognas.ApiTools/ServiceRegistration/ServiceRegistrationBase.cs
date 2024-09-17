using Cognas.ApiTools.ServiceRegistration.Abstractions;
using Cognas.Tools.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;
using System.Reflection;

namespace Cognas.ApiTools.ServiceRegistration;

/// <summary>
/// 
/// </summary>
public abstract class ServiceRegistrationBase : IServiceRegistration
{
    #region Field Declarations

    private static readonly Lazy<FrozenSet<Type>> _types = new(() => GetNonAbstractClassesInternal());

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
    protected static FrozenSet<Type> GetNonAbstractClasses(Assembly? assembly = null) => assembly == null ? _types.Value : GetNonAbstractClassesInternal(assembly);

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private static FrozenSet<Type> GetNonAbstractClassesInternal(Assembly? assembly = null)
    {
        List<Type> types = [];
        Assembly assemblyForTypes = assembly ?? Assembly.GetEntryAssembly()!;
        assemblyForTypes.GetTypes().FastForEach(type => type.IsClass && !type.IsAbstract, type => types.Add(type));
        return types.ToFrozenSet();
    }

    #endregion
}