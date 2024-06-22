namespace Cognas.ApiTools.Mapping;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryMappingService<TModel, TResponse>
    where TModel : class
    where TResponse : class
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    TResponse ModelToResponse(TModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    IEnumerable<TResponse> ModelsToResponses(IEnumerable<TModel> models);

    #endregion
}