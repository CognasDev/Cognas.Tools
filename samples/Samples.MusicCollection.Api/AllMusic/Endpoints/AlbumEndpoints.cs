using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Mvc;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.AllMusic.Extensions;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class AlbumEndpoints : MicroserviceEndpointsBase<Album>, IEndpoints
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
            $"/{Uri}",
            (
                CancellationToken cancellationToken,
                [AsParameters] PaginationQuery paginationQuery
            ) => _albumMicroserviceBusinessLogic.Get(paginationQuery, cancellationToken)
        )
        .MapGetConfiguration<AlbumResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapGetById(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet
        (
            $"/{Uri}/{{id}}",
            async ([FromRoute] int id) => await _albumMicroserviceBusinessLogic.GetByIdAsync(id).ConfigureAwait(false)
        )
        .MapGetByIdConfiguration<AlbumResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapPost(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost
        (
            $"/{Uri}",
            async ([FromBody] AlbumRequest request) => await _albumMicroserviceBusinessLogic.PostAsync(request).ConfigureAwait(false)
        )
        .MapPostConfiguration<AlbumRequest, AlbumResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapPut(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut
        (
            $"/{Uri}/{{id}}",
            async ([FromRoute] int id, [FromBody] AlbumRequest request) => await _albumMicroserviceBusinessLogic.PutAsync(request).ConfigureAwait(false)
        )
        .MapPutConfiguration<AlbumRequest, AlbumResponse>(ApiVersion, Tag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapDelete(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete
        (
            $"/{Uri}/{{id}}",
            async ([FromRoute] int id) => await _albumMicroserviceBusinessLogic.DeleteAsync(id).ConfigureAwait(false)
        )
        .MapDeleteConfiguration(ApiVersion, Tag);
    }

    #endregion
}