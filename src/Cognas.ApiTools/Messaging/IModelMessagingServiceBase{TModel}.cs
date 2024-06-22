namespace Cognas.ApiTools.Messaging;

/// <summary>
/// 
/// </summary>
public interface IModelMessagingService<TModel> where TModel : class
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task OnInsertModelAsync(TModel? model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task OnUpdateModelAsync(TModel? model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task OnDeleteModelAsync(TModel model);

    #endregion
}