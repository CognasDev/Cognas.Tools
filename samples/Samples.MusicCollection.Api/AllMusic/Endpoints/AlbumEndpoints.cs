using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Mvc;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using System.Net.Mime;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class AlbumEndpoints : IAlbumEndpoints
{
    #region Field Declarations

    private readonly IAlbumMicroserviceBusinessLogic _albumMicroserviceBusinessLogic;

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
            $"/allmusic/albums",
            (
                CancellationToken cancellationToken,
                [AsParameters] PaginationQuery paginationQuery
            ) =>
            {
                return _albumMicroserviceBusinessLogic.GetAlbums(paginationQuery, cancellationToken);
            }
        )
        .MapToApiVersion(1)
        .WithTags("- All Music")
        .WithOpenApi()
        .Produces<IEnumerable<AlbumResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapGetById(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet
        (
            $"/allmusic/albums/{{id}}",
            async
            (
                [FromRoute] int id
            ) =>
            {
                return await _albumMicroserviceBusinessLogic.GetAlbumByAlbumIdAsync(id).ConfigureAwait(false);
            }
        )
        .MapToApiVersion(1)
        .WithTags("- All Music")
        .WithOpenApi()
        .Produces<AlbumResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    #endregion
}