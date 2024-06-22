using Cognas.ApiTools.Shared;
using System.Linq.Expressions;

namespace Cognas.ApiTools.Data;

/// <summary>
/// 
/// </summary>
public interface IModelParameter<TModel> : IParameter where TModel : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    Expression<Func<TModel, object?>> PropertyExpression { get; }

    #endregion

    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Func<TModel, object?> GetFunction();

    #endregion
}