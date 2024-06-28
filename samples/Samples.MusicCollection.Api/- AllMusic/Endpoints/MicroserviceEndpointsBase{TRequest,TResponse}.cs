using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.AllMusic.Extensions;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class MicroserviceEndpointsBase<TRequest, TResponse> : IMicroserviceEndpoints where TRequest : notnull
{
    #region Field Declarations

    private readonly IOptions<AllMusicRoutes> _routes;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IMicroserviceBusinessLogic<TRequest, TResponse> BusinessLogic { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual int ApiVersion { get; } = 2;

    /// <summary>
    /// 
    /// </summary>
    public virtual string Tag { get; } = "All Music";

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MicroserviceEndpointsBase{TRequest,TResponse}"/>
    /// </summary>
    /// <param name="businessLogic"></param>
    /// <param name="routes"></param>
    protected MicroserviceEndpointsBase(IMicroserviceBusinessLogic<TRequest, TResponse> businessLogic,
                                        IOptions<AllMusicRoutes> routes)
    {
        ArgumentNullException.ThrowIfNull(businessLogic, nameof(businessLogic));
        ArgumentNullException.ThrowIfNull(routes, nameof(routes));

        BusinessLogic = businessLogic;
        _routes = routes;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    public abstract string Route(AllMusicRoutes routes);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapGet(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapGet
        (
            $"/{Route(_routes.Value)}",
            (
                CancellationToken cancellationToken,
                [AsParameters] PaginationQuery paginationQuery
            ) => BusinessLogic.Get(paginationQuery, cancellationToken)
        ).MapGetConfiguration<TResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapGetById(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapGet
        (
            $"/{Route(_routes.Value)}/{{id}}",
            async ([FromRoute] int id) => await BusinessLogic.GetByIdAsync(id).ConfigureAwait(false)
        )
        .MapGetByIdConfiguration<TResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapPost(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapPost
        (
            $"/{Route(_routes.Value)}",
            async ([FromBody] TRequest request) => await BusinessLogic.PostAsync(request).ConfigureAwait(false)
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
            $"/{Route(_routes.Value)}/{{id}}",
            async ([FromRoute] int id, [FromBody] TRequest request) => await BusinessLogic.PutAsync(request).ConfigureAwait(false)
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
            $"/{Route(_routes.Value)}/{{id}}",
            async ([FromRoute] int id) => await BusinessLogic.DeleteAsync(id).ConfigureAwait(false)
        )
        .MapDeleteConfiguration(ApiVersion, Tag);
    }

    #endregion
}