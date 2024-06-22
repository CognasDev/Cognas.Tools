namespace Cognas.ApiTools.Shared;

/// <summary>
/// 
/// </summary>
public interface IModelIdService
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    int GetId<TModel>(TModel model) where TModel : class;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    void SetId<TModel>(TModel model, int id) where TModel : class;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    IParameter IdParameter<TModel>(int id) where TModel : class;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    string GetModelIdName<TModel>() where TModel : class;

    #endregion
}