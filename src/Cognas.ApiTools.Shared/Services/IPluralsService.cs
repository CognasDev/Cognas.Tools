namespace Cognas.ApiTools.Shared.Services;

/// <summary>
/// 
/// </summary>
public interface IPluralsService
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    string PluraliseModelName<TModel>() where TModel : class;

    #endregion
}