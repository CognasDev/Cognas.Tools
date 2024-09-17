using Cognas.ApiTools.Data.Command;
using Cognas.ApiTools.Data.Exceptions;
using Cognas.ApiTools.Messaging;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.Shared.Services;
using LanguageExt.Common;
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
    public async Task<Result<TModel>> InsertModelAsync(TModel model)
    {
        TModel? insertedModel = await DatabaseService.InsertModelAsync(InsertStoredProcedure, model).ConfigureAwait(false);
        if (insertedModel is not null)
        {
            if (_useMessaging)
            {
                await ModelMessagingService!.OnInsertModelAsync(insertedModel).ConfigureAwait(false);
            }
            return insertedModel;
        }
        return new Result<TModel>(new InsertModelException<TModel>(model));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Result<TModel>> UpdateModelAsync(TModel model)
    {
        TModel? updatedModel = await DatabaseService.UpdateModelAsync(UpdateStoredProcedure, model).ConfigureAwait(false);
        if (updatedModel is not null)
        {
            if (_useMessaging)
            {
                await ModelMessagingService!.OnUpdateModelAsync(updatedModel).ConfigureAwait(false);
            }
            return updatedModel;
        }
        return new Result<TModel>(new UpdateModelException<TModel>(model));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<Result<bool>> DeleteModelAsync(params IParameter[] parameters)
    {
        int deleteCount = await DatabaseService.DeleteModelAsync(DeleteStoredProcedure, parameters).ConfigureAwait(false);
        return deleteCount == 1 ? true : new Result<bool>(new DeleteModelException<TModel>());
    }

    #endregion
}