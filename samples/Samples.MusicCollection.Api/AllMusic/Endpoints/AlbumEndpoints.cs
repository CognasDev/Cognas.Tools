using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.AllMusic.Extensions;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class AlbumEndpoints : IAlbumEndpoints
{
    #region Field Declarations

    private readonly IAlbumMicroserviceBusinessLogic _albumMicroserviceBusinessLogic;
    private const string _route = "allmusic";
    private const string _tag = "All Music";
    private readonly string _albums;
    private const int _apiVersion = 3;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumEndpoints"/>
    /// </summary>
    /// <param name="albumMicroserviceBusinessLogic"></param>
    public AlbumEndpoints(IAlbumMicroserviceBusinessLogic albumMicroserviceBusinessLogic)
    {
        ArgumentNullException.ThrowIfNull(albumMicroserviceBusinessLogic, nameof(albumMicroserviceBusinessLogic));
        _albumMicroserviceBusinessLogic = albumMicroserviceBusinessLogic;
        _albums = PluralsService.Instance.PluraliseModelName<Album>().ToLowerInvariant();
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapGet(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet
        (
            $"/{_route}/{_albums}",
            (
                CancellationToken cancellationToken,
                [AsParameters] PaginationQuery paginationQuery
            ) =>
            {
                return _albumMicroserviceBusinessLogic.Get(paginationQuery, cancellationToken);
            }
        )
        .MapGetConfiguration<AlbumResponse>(_apiVersion, _tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapGetById(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet
        (
            $"/{_route}/{_albums}/{{id}}",
            async
            (
                [FromRoute] int id
            ) =>
            {
                return await _albumMicroserviceBusinessLogic.GetByIdAsync(id).ConfigureAwait(false);
            }
        )
        .MapGetByIdConfiguration<AlbumResponse>(_apiVersion, _tag);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapPost(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost
        (
            $"/{_route}/{_albums}",
            async
            (
                HttpContext httpContext,
                [FromBody] AlbumRequest request
            ) =>
            {
                return await _albumMicroserviceBusinessLogic.PostAsync(request).ConfigureAwait(false);
            }
        )
        .MapPostConfiguration<AlbumRequest, AlbumResponse>(_apiVersion, _tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapPut(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut
        (
            $"/{_route}/{_albums}/{{id}}",
            async
            (
                [FromRoute] int id,
                [FromBody] AlbumRequest request
            ) =>
            {
                return await _albumMicroserviceBusinessLogic.PutAsync(request).ConfigureAwait(false);
            }
        )
        .MapPutConfiguration<AlbumRequest, AlbumResponse>(_apiVersion, _tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapDelete(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete
        (
            $"/{_route}/{_albums}/{{id}}",
            async
            (
                [FromRoute] int id
            ) =>
            {
                await _albumMicroserviceBusinessLogic.DeleteAsync(id).ConfigureAwait(false);
            }
        )
        .MapDeleteConfiguration(_apiVersion, _tag);
    }

    #endregion
}