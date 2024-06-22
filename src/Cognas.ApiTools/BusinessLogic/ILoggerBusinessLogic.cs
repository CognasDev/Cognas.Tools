using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
public interface ILoggerBusinessLogic
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    ILogger Logger { get; }

    #endregion
}