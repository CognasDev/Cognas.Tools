using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Responses;
using Samples.MusicCollection.Api.Config;
using System.Net.Mime;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class AllMusicEndpoints : IAllMusicEndpoints
{
    #region Field Declarations

    private readonly IAllMusicBusinessLogic _businessLogic;
    private readonly IOptions<AllMusicRoutes> _routes;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AllMusicEndpoints"/>
    /// </summary>
    /// <param name="businessLogic"></param>
    /// <param name="routes"></param>
    public AllMusicEndpoints(IAllMusicBusinessLogic businessLogic, IOptions<AllMusicRoutes> routes)
    {
        ArgumentNullException.ThrowIfNull(businessLogic, nameof(businessLogic));
        ArgumentNullException.ThrowIfNull(routes, nameof(routes));
        _businessLogic = businessLogic;
        _routes = routes;
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
            $"/{_routes.Value.Home}",
            async (CancellationToken cancellationToken) => await _businessLogic.GetAllMusicAsync(cancellationToken).ConfigureAwait(false)
        )
        .MapToApiVersion(2)
        .WithTags(MicroserviceTags.AllMusic)
        .WithOpenApi()
        .Produces<AllMusicResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    #endregion
}