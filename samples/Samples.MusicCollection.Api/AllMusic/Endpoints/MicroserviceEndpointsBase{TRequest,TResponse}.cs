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
public abstract class MicroserviceEndpointsBase<TRequest, TResponse> : IDisposable, IMicroserviceEndpoints where TRequest : notnull
{
    #region Field Declarations

    private AllMusicRoutes _routes;
    private readonly IDisposable? _routesChangedListener;
    private bool _isDisposed;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IMicroserviceBusinessLogic<TRequest, TResponse> BusinessLogic { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual int ApiVersion { get; } = 3;

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
    /// <param name="routesMonitor"></param>
    protected MicroserviceEndpointsBase(IMicroserviceBusinessLogic<TRequest, TResponse> businessLogic,
                                        IOptionsMonitor<AllMusicRoutes> routesMonitor)
    {
        ArgumentNullException.ThrowIfNull(businessLogic, nameof(businessLogic));
        ArgumentNullException.ThrowIfNull(routesMonitor, nameof(routesMonitor));

        BusinessLogic = businessLogic;
        _routesChangedListener = routesMonitor.OnChange(OnRoutesChanged);
        _routes = routesMonitor.CurrentValue;
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
    public void MapGet(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet
        (
            $"/{Route(_routes)}",
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
    public void MapGetById(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet
        (
            $"/{Route(_routes)}/{{id}}",
            async ([FromRoute] int id) => await BusinessLogic.GetByIdAsync(id).ConfigureAwait(false)
        )
        .MapGetByIdConfiguration<TResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapPost(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost
        (
            $"/{Route(_routes)}",
            async ([FromBody] TRequest request) => await BusinessLogic.PostAsync(request).ConfigureAwait(false)
        )
        .MapPostConfiguration<TRequest, TResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapPut(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut
        (
            $"/{Route(_routes)}/{{id}}",
            async ([FromRoute] int id, [FromBody] TRequest request) => await BusinessLogic.PutAsync(request).ConfigureAwait(false)
        )
        .MapPutConfiguration<TRequest, TResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapDelete(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete
        (
            $"/{Route(_routes)}/{{id}}",
            async ([FromRoute] int id) => await BusinessLogic.DeleteAsync(id).ConfigureAwait(false)
        )
        .MapDeleteConfiguration(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routes"></param>
    private void OnRoutesChanged(AllMusicRoutes routes) => _routes = routes;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
        {
            _routesChangedListener?.Dispose();
        }
        _isDisposed = true;
    }

    #endregion
}