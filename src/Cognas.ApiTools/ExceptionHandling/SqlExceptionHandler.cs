using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Cognas.ApiTools.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public sealed class SqlExceptionHandler : ExceptionHandlerBase<SqlException>
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public override int StatusCode => StatusCodes.Status500InternalServerError;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="SqlExceptionHandler"/>
    /// </summary>
    /// <param name="logger"></param>
    public SqlExceptionHandler(ILogger<SqlExceptionHandler> logger) : base(logger)
    {
    }

    #endregion
}