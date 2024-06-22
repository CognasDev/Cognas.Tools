namespace Cognas.ApiTools.SourceGenerators.Abstractions;

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
    int GetIdValue<TModel>(TModel model) where TModel : class;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    void SetIdValue<TModel>(TModel model, int id) where TModel : class;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    IParameter IdParameter<TModel>(int id) where TModel : class;

    #endregion
}