using Cognas.ApiTools.ExceptionHandling;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data.SqlClient;

namespace ApiTools.UnitTests.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public sealed class SqlExceptionHandlerTests
{
    #region Unit Test Declarations

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void ProblemDetailsTitle()
    {
        ILogger<SqlExceptionHandler> logger = Mock.Of<ILogger<SqlExceptionHandler>>();
        SqlExceptionHandler sqlExceptionHandler = new(logger);
        sqlExceptionHandler.ProblemDetailsTitle.Should().Be(typeof(SqlException).Name);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void StatusCode()
    {
        ILogger<SqlExceptionHandler> logger = Mock.Of<ILogger<SqlExceptionHandler>>();
        SqlExceptionHandler sqlExceptionHandler = new(logger);
        sqlExceptionHandler.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    #endregion
}