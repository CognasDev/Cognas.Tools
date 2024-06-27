using Cognas.ApiTools.Pagination;
using Microsoft.AspNetCore.Mvc;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.AllMusic.Extensions;
using Samples.MusicCollection.Api.Artists;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class ArtistEndpoints : MicroserviceEndpointsBase<Artist>, IEndpoints
{
    #region Field Declarations

    private readonly IArtistMicroserviceBusinessLogic _artistMicroserviceBusinessLogic;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistEndpoints"/>
    /// </summary>
    /// <param name="artistMicroserviceBusinessLogic"></param>
    public ArtistEndpoints(IArtistMicroserviceBusinessLogic artistMicroserviceBusinessLogic)
    {
        ArgumentNullException.ThrowIfNull(artistMicroserviceBusinessLogic, nameof(artistMicroserviceBusinessLogic));
        _artistMicroserviceBusinessLogic = artistMicroserviceBusinessLogic;
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
            ) => _artistMicroserviceBusinessLogic.Get(paginationQuery, cancellationToken)
        )
        .MapGetConfiguration<ArtistResponse>(ApiVersion, Tag);
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
            async ([FromRoute] int id) => await _artistMicroserviceBusinessLogic.GetByIdAsync(id).ConfigureAwait(false)
        )
        .MapGetByIdConfiguration<ArtistResponse>(ApiVersion, Tag);
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
            async ([FromBody] ArtistRequest request) => await _artistMicroserviceBusinessLogic.PostAsync(request).ConfigureAwait(false)
        )
        .MapPostConfiguration<ArtistRequest, ArtistResponse>(ApiVersion, Tag);
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
            async ([FromRoute] int id, [FromBody] ArtistRequest request) => await _artistMicroserviceBusinessLogic.PutAsync(request).ConfigureAwait(false)
        )
        .MapPutConfiguration<ArtistRequest, ArtistResponse>(ApiVersion, Tag);
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
            async ([FromRoute] int id) => await _artistMicroserviceBusinessLogic.DeleteAsync(id).ConfigureAwait(false)
        )
        .MapDeleteConfiguration(ApiVersion, Tag);
    }

    #endregion
}