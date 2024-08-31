using Cognas.ApiTools.Shared;
using Dapper;

namespace Cognas.ApiTools.Data;

/// <summary>
/// 
/// </summary>
public interface IDynamicParameterFactory
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    DynamicParameters? Create(IReadOnlyList<IParameter> parameters);

    #endregion
}