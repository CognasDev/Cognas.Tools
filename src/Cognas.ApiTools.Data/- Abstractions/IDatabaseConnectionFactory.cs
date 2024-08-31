using System.Data;

namespace Cognas.ApiTools.Data;

/// <summary>
/// 
/// </summary>
public interface IDatabaseConnectionFactory
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IDbConnection Create();

    #endregion
}