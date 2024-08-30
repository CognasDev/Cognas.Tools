using Cognas.ApiTools.Data.Command;
using Cognas.ApiTools.Messaging;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public abstract class CommandBusinessLogicBase<TModel> : ModelIdServiceBusinessLogic, ICommandBusinessLogic<TModel>
    where TModel : class
{
    #region Field Declarations

    private readonly bool _useMessaging;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public ICommandDatabaseService DatabaseService { get; }

    /// <summary>
    /// 
    /// </summary>
    public IModelMessagingService<TModel>? ModelMessagingService { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string InsertStoredProcedure { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string UpdateStoredProcedure { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string DeleteStoredProcedure { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandBusinessLogicBase{TModel}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="modelIdService"></param>
    /// <param name="databaseService"></param>
    /// <param name="modelMessagingService"></param>
    protected CommandBusinessLogicBase(ILogger logger,
                                       IModelIdService modelIdService,
                                       ICommandDatabaseService databaseService,
                                       IModelMessagingService<TModel>? modelMessagingService = null)
        : base(logger, modelIdService)
    {
        ArgumentNullException.ThrowIfNull(databaseService, nameof(databaseService));
        DatabaseService = databaseService;

        _useMessaging = modelMessagingService is not null;
        if (_useMessaging)
        {
            ModelMessagingService = modelMessagingService;
        }

        string pluralModelName = PluralsService.Instance.PluraliseModelName<TModel>();
        InsertStoredProcedure = $"[dbo].[{pluralModelName}_Insert]";
        UpdateStoredProcedure = $"[dbo].[{pluralModelName}_Update]";
        DeleteStoredProcedure = $"[dbo].[{pluralModelName}_Delete]";
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<TModel?> InsertModelAsync(TModel model)
    {
        TModel? insertedModel = await DatabaseService.InsertModelAsync(InsertStoredProcedure, model).ConfigureAwait(false);
        if (_useMessaging && insertedModel is not null)
        {
            await ModelMessagingService!.OnInsertModelAsync(insertedModel).ConfigureAwait(false);
        }
        return insertedModel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<TModel?> UpdateModelAsync(TModel model)
    {
        TModel? updatedModel = await DatabaseService.UpdateModelAsync(UpdateStoredProcedure, model).ConfigureAwait(false);
        if (_useMessaging && updatedModel is not null)
        {
            await ModelMessagingService!.OnUpdateModelAsync(updatedModel).ConfigureAwait(false);
        }
        return updatedModel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<bool> DeleteModelAsync(params IParameter[] parameters)
    {
        int deleteCount = await DatabaseService.DeleteModelAsync(DeleteStoredProcedure, parameters).ConfigureAwait(false);
        bool success = deleteCount == 1;
        return success;
    }

    #endregion
}