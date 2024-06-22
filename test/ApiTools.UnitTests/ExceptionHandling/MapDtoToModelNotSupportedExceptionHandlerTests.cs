using Cognas.ApiTools.ExceptionHandling;
using Cognas.ApiTools.Mapping;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiTools.UnitTests.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public sealed class MapDtoToModelNotSupportedExceptionHandlerTests
{
    #region Unit Test Declarations

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void ProblemDetailsTitle()
    {
        ILogger<MapDtoToModelNotSupportedExceptionHandler> logger = Mock.Of<ILogger<MapDtoToModelNotSupportedExceptionHandler>>();
        MapDtoToModelNotSupportedExceptionHandler mapDtoToModelNotSupportedExceptionHandler = new(logger);
        mapDtoToModelNotSupportedExceptionHandler.ProblemDetailsTitle.Should().Be(typeof(MapRequestToModelNotSupportedException).Name);
    }

    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public void StatusCode()
    {
        ILogger<MapDtoToModelNotSupportedExceptionHandler> logger = Mock.Of<ILogger<MapDtoToModelNotSupportedExceptionHandler>>();
        MapDtoToModelNotSupportedExceptionHandler mapDtoToModelNotSupportedExceptionHandler = new(logger);
        mapDtoToModelNotSupportedExceptionHandler.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    #endregion
}