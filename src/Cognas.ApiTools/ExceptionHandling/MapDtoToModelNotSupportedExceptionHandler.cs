using Cognas.ApiTools.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public sealed class MapDtoToModelNotSupportedExceptionHandler : ExceptionHandlerBase<MapRequestToModelNotSupportedException>
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public override int StatusCode => StatusCodes.Status500InternalServerError;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MapRequestToModelNotSupportedException"/>
    /// </summary>
    /// <param name="logger"></param>
    public MapDtoToModelNotSupportedExceptionHandler(ILogger<MapDtoToModelNotSupportedExceptionHandler> logger) : base(logger)
    {
    }

    #endregion
}