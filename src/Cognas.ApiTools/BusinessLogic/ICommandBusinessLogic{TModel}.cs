using Cognas.ApiTools.Data.Command;
using Cognas.ApiTools.Messaging;
using Cognas.ApiTools.Shared;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface ICommandBusinessLogic<TModel> : ILoggerBusinessLogic, IModelIdServiceBusinessLogic where TModel : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    ICommandDatabaseService DatabaseService { get; }

    /// <summary>
    /// 
    /// </summary>
    IModelMessagingService<TModel>? ModelMessagingService { get; }

    /// <summary>
    /// 
    /// </summary>
    string InsertStoredProcedure { get; }

    /// <summary>
    /// 
    /// </summary>
    string UpdateStoredProcedure { get; }

    /// <summary>
    /// 
    /// </summary>
    string DeleteStoredProcedure { get; }

    #endregion

    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<TModel?> InsertModelAsync(TModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<TModel?> UpdateModelAsync(TModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<bool> DeleteModelAsync(params IParameter[] parameters);

    #endregion
}