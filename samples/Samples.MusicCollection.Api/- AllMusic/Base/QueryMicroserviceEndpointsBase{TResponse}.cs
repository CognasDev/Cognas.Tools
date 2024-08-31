﻿using Cognas.ApiTools.Microservices;
using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    public virtual string Tag { get; } = MicroserviceTags.Models;

    /// <summary>
    /// 
    /// </summary>
    public AllMusicRoutes AllMusicRoutes { get; }

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
        AllMusicRoutes = routes.Value;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="allMusicRoutes"></param>
    /// <returns></returns>
    public abstract string GetRoute(AllMusicRoutes allMusicRoutes);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public virtual RouteHandlerBuilder MapGet(IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapGet
        (
            $"/{GetRoute(AllMusicRoutes)}",
            (
                CancellationToken cancellationToken, [AsParameters] PaginationQuery paginationQuery
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
            $"/{GetRoute(AllMusicRoutes)}/{{id}}",
            async Task<Results<Ok<TResponse>, NotFound>> ([FromRoute] int id) =>
            {
                TResponse? response = await QueryBusinessLogic.GetByIdAsync(id).ConfigureAwait(false);
                return response is not null ? TypedResults.Ok(response) : TypedResults.NotFound();
            }
        )
        .MapGetByIdConfiguration<TResponse>(ApiVersion, Tag);
    }

    #endregion
}