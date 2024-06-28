using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Extensions;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Base;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public abstract class QueryMicroserviceEndpointsBase<TResponse> : IQueryMicroserviceEndpoints where TResponse : class
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IQueryMicroserviceBusinessLogic<TResponse> QueryBusinessLogic { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual int ApiVersion { get; } = 2;

    /// <summary>
    /// 
    /// </summary>
    public virtual string Tag { get; } = "All Music";

    /// <summary>
    /// 
    /// </summary>
    public IOptions<AllMusicRoutes> Routes { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryMicroserviceEndpointsBase{TResponse}"/>
    /// </summary>
    /// <param name="queryBusinessLogic"></param>
    /// <param name="routes"></param>
    protected QueryMicroserviceEndpointsBase(IQueryMicroserviceBusinessLogic<TResponse> queryBusinessLogic,
                                             IOptions<AllMusicRoutes> routes)
    {
        ArgumentNullException.ThrowIfNull(queryBusinessLogic, nameof(queryBusinessLogic));
        ArgumentNullException.ThrowIfNull(routes, nameof(routes));

        QueryBusinessLogic = queryBusinessLogic;
        Routes = routes;
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
            $"/{Route(Routes.Value)}",
            (
                CancellationToken cancellationToken,
                [AsParameters] PaginationQuery paginationQuery
            ) => QueryBusinessLogic.Get(paginationQuery, cancellationToken)
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
            $"/{Route(Routes.Value)}/{{id}}",
            async ([FromRoute] int id) => await QueryBusinessLogic.GetByIdAsync(id).ConfigureAwait(false)
        )
        .MapGetByIdConfiguration<TResponse>(ApiVersion, Tag);
    }

    #endregion
}