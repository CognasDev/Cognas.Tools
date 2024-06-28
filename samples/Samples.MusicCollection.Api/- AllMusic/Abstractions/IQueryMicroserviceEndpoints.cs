namespace Samples.MusicCollection.Api.AllMusic.Abstractions;

/// <summary>
/// 
/// </summary>
public interface IQueryMicroserviceEndpoints
{
    #region Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    RouteHandlerBuilder MapGet(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    RouteHandlerBuilder MapGetById(IEndpointRouteBuilder endpointRouteBuilder);

    #endregion
}