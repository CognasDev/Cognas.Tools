using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public sealed class OperationCanceledExceptionHandler : ExceptionHandlerBase<OperationCanceledException>
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public override int StatusCode => StatusCodes.Status500InternalServerError;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="OperationCanceledExceptionHandler"/>
    /// </summary>
    /// <param name="logger"></param>
    public OperationCanceledExceptionHandler(ILogger<OperationCanceledExceptionHandler> logger) : base(logger)
    {
    }

    #endregion
}