using Cognas.ApiTools.Data.Query;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Extensions;
using Cognas.ApiTools.Shared.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public abstract class QueryBusinessLogicBase<TModel> : CommandOrQueryBusinessLogicBase, IQueryBusinessLogic<TModel> where TModel : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IMemoryCache MemoryCache { get; }

    /// <summary>
    /// 
    /// </summary>
    public string CacheKey => typeof(TModel).Name;

    /// <summary>
    /// 
    /// </summary>
    public virtual int CacheTimeOutMinutes => 30;

    /// <summary>
    /// 
    /// </summary>
    public virtual bool UseCache { get; } = true;

    /// <summary>
    /// 
    /// </summary>
    public IQueryDatabaseService DatabaseService { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string SelectStoredProcedure { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string SelectByIdStoredProcedure { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryBusinessLogicBase{TModel}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="memoryCache"></param>
    /// <param name="modelIdService"></param>
    /// <param name="databaseService"></param>
    protected QueryBusinessLogicBase(ILogger logger, IMemoryCache memoryCache, IModelIdService modelIdService, IQueryDatabaseService databaseService)
        : base(logger, modelIdService)
    {
        ArgumentNullException.ThrowIfNull(memoryCache, nameof(memoryCache));
        ArgumentNullException.ThrowIfNull(databaseService, nameof(databaseService));

        MemoryCache = memoryCache;
        DatabaseService = databaseService;

        string pluralModelName = PluralsService.Instance.PluraliseModelName<TModel>();
        SelectStoredProcedure = $"[dbo].[{pluralModelName}_Select]";
        SelectByIdStoredProcedure = $"[dbo].[{pluralModelName}_SelectById]";
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<IEnumerable<TModel>> SelectModelsAsync()
    {
        IEnumerable<TModel>? selectedModels;
        if (UseCache)
        {
            selectedModels = await MemoryCache.GetOrCreateAsync(CacheKey, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeOutMinutes);
                return SelectModelsFromDatabaseAsync();
            }).ConfigureAwait(false);
        }
        else
        {
            selectedModels = await SelectModelsFromDatabaseAsync().ConfigureAwait(false);
        }
        return selectedModels ?? throw new NullReferenceException($"Model: {typeof(TModel).Name}, {nameof(SelectStoredProcedure)}: {SelectStoredProcedure}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idExpression"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<TModel?> SelectModelAsync(int id, IParameter idExpression)
    {
        TModel? selectedModel;
        if (UseCache)
        {
            IEnumerable<TModel> models = await SelectModelsAsync().ConfigureAwait(false);
            selectedModel = models.FastFirstOrDefault(model => GetModelById(model, id));
        }
        else
        {
            selectedModel = await DatabaseService.SelectModelAsync<TModel>(SelectByIdStoredProcedure, idExpression).ConfigureAwait(false);
        }
        return selectedModel;
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task ResetCacheAsync()
    {
        if (UseCache)
        {
            await Task.Run(() => MemoryCache.Remove(CacheKey)).ConfigureAwait(false);
        }
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<TModel>> SelectModelsFromDatabaseAsync() => await DatabaseService.SelectModelsAsync<TModel>(SelectStoredProcedure).ConfigureAwait(false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool GetModelById(TModel model, int id) => ModelIdService.GetId<TModel>(model) == id;

    #endregion
}