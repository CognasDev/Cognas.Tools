namespace Cognas.ApiTools.Shared.Patterns;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TIType"></typeparam>
public interface IFactory<TIType>
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TCreate"></typeparam>
    /// <returns></returns>
    TIType Create<TCreate>() where TCreate : TIType;

    #endregion
}