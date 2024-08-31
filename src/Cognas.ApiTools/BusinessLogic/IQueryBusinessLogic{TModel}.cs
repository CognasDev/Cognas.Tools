using Cognas.ApiTools.Data.Query;
using Cognas.ApiTools.Shared;
using LanguageExt;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IQueryBusinessLogic<TModel> : ICacheBusinessLogic, ILoggerBusinessLogic, IModelIdServiceBusinessLogic where TModel : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    IQueryDatabaseService DatabaseService { get; }

    /// <summary>
    /// 
    /// </summary>
    string SelectStoredProcedure { get; }

    /// <summary>
    /// 
    /// </summary>
    string SelectByIdStoredProcedure { get; }

    #endregion

    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TModel>> SelectModelsAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idParameter"></param>
    /// <returns></returns>
    Task<Option<TModel>> SelectModelAsync(int id, IParameter idParameter);

    #endregion
}