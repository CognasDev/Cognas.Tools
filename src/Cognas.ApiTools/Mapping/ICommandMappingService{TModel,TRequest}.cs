namespace Cognas.ApiTools.Mapping;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
public interface ICommandMappingService<TModel, TRequest>
    where TModel : class
    where TRequest : notnull
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    TModel RequestToModel(TRequest request);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requests"></param>
    /// <returns></returns>
    IEnumerable<TModel> RequestsToModels(IEnumerable<TRequest> requests);

    #endregion
}