using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Microservices;
using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class CommandQueryMicroserviceEndpointsBase<TRequest, TResponse> :
    QueryMicroserviceEndpointsBase<TResponse>, ICommandMicroserviceEndpoints
    where TRequest : notnull
    where TResponse : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public ICommandMicroserviceBusinessLogic<TRequest, TResponse> CommandBusinessLogic { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandQueryMicroserviceEndpointsBase{TRequest,TResponse}"/>
    /// </summary>
    /// <param name="commandBusinessLogic"></param>
    /// <param name="queryBusinessLogic"></param>
    /// <param name="routes"></param>
    protected CommandQueryMicroserviceEndpointsBase(ICommandMicroserviceBusinessLogic<TRequest, TResponse> commandBusinessLogic,
                                                    IQueryMicroserviceBusinessLogic<TResponse> queryBusinessLogic,
                                                    IOptions<AllMusicRoutes> routes) : base(queryBusinessLogic, routes)
    {
        ArgumentNullException.ThrowIfNull(commandBusinessLogic, nameof(commandBusinessLogic));
        CommandBusinessLogic = commandBusinessLogic;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapPost(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapPost
        (
            $"/{GetRoute(AllMusicRoutes)}",
            async Task<Results<Created<TResponse>, BadRequest>> (HttpContext httpContext, [FromBody] TRequest request) =>
            {
                LocationResponse<TResponse> locationResponse = await CommandBusinessLogic.PostAsync(request).ConfigureAwait(false);
                if (locationResponse.Success)
                {
                    string route = GetRoute(AllMusicRoutes);
                    string locationUri = httpContext.BuildLocationUri(route, locationResponse.Id!.Value);
                    return TypedResults.Created(locationUri, locationResponse.Response!);
                }
                return TypedResults.BadRequest();
            }
        )
        .MapPostConfiguration<TRequest, TResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapPut(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapPut
        (
            $"/{GetRoute(AllMusicRoutes)}/{{id}}",
            async Task<Results<Ok<TResponse>, BadRequest>> ([FromRoute] int id, [FromBody] TRequest request) =>
            {
                TResponse? response = await CommandBusinessLogic.PutAsync(request).ConfigureAwait(false);
                return response is not null ? TypedResults.Ok(response) : TypedResults.BadRequest();
            }
        )
        .MapPutConfiguration<TRequest, TResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapDelete(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapDelete
        (
            $"/{GetRoute(AllMusicRoutes)}/{{id}}",
            async Task<Ok> ([FromRoute] int id) =>
            {
                await CommandBusinessLogic.DeleteAsync(id).ConfigureAwait(false);
                return TypedResults.Ok();
            }
        )
        .MapDeleteConfiguration(ApiVersion, Tag);
    }

    #endregion
}