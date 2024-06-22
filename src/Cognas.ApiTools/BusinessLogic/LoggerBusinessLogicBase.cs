using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
public abstract class LoggerBusinessLogicBase : ILoggerBusinessLogic
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public ILogger Logger { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="LoggerBusinessLogicBase"/>
    /// </summary>
    /// <param name="logger"></param>
    protected LoggerBusinessLogicBase(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        Logger = logger;
    }

    #endregion
}