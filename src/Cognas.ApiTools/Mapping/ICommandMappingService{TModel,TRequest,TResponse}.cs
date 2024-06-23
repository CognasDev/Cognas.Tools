namespace Cognas.ApiTools.Mapping;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandMappingService<TModel, TRequest, TResponse> 
    where TModel : class
    where TRequest : class
    where TResponse : class
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