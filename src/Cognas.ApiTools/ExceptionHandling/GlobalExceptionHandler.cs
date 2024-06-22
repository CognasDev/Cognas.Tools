using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public sealed class GlobalExceptionHandler : ExceptionHandlerBase<Exception>
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public override int StatusCode => StatusCodes.Status500InternalServerError;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GlobalExceptionHandler"/>
    /// </summary>
    /// <param name="logger"></param>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : base(logger)
    {
    }

    #endregion
}