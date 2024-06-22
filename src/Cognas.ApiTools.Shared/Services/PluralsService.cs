using Pluralize.NET;
using System.Collections.Concurrent;

namespace Cognas.ApiTools.Shared.Services;

/// <summary>
/// 
/// </summary>
public sealed class PluralsService : IPluralsService
{
    #region Field Declarations

    private static readonly Lazy<IPluralsService> _lazyInstance = new(() => new PluralsService());
    private static readonly ConcurrentDictionary<Type, string> _modelPluralsCache = [];
    private static Pluralizer? _pluralize = null;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public static IPluralsService Instance => _lazyInstance.Value;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="PluralsService"/>
    /// </summary>
    private PluralsService() => _pluralize = new Pluralizer();

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    public string PluraliseModelName<TModel>() where TModel : class => _modelPluralsCache.GetOrAdd(typeof(TModel), key => _pluralize!.Pluralize(key.Name));

    #endregion
}