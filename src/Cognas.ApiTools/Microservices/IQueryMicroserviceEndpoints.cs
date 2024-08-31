using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Cognas.ApiTools.Microservices;

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