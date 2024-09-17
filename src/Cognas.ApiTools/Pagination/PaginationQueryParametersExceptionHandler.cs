using Cognas.ApiTools.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.Pagination;

/// <summary>
/// 
/// </summary>
public sealed class PaginationQueryParametersExceptionHandler : ExceptionHandlerBase<PaginationQueryParametersException>
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public override int StatusCode => StatusCodes.Status400BadRequest;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="PaginationQueryParametersExceptionHandler"/>
    /// </summary>
    /// <param name="logger"></param>
    public PaginationQueryParametersExceptionHandler(ILogger<PaginationQueryParametersExceptionHandler> logger) : base(logger)
    {
    }

    #endregion
}