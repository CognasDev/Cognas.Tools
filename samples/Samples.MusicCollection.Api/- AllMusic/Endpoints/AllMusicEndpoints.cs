using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Requests;
using Samples.MusicCollection.Api.AllMusic.Responses;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Tracks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class AllMusicEndpoints : IAllMusicEndpoints
{
    #region Field Declarations

    private readonly IAllMusicBusinessLogic _businessLogic;
    private readonly AllMusicRoutes _routes;

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
        _routes = routes.Value;
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
            $"/{_routes.Home}",
            async (CancellationToken cancellationToken) => await _businessLogic.GetAllMusicAsync(cancellationToken).ConfigureAwait(false)
        )
        .MapToApiVersion(2)
        .WithTags(MicroserviceTags.AllMusic)
        .WithOpenApi()
        .Produces<AllMusicResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    public void MapPostAreMixableTracks(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost($"/{_routes.Home}/aremixabletracks", ([FromBody] IEnumerable<MixableTrackRequest> tracks) =>
        {
            if (tracks.Count() < 2)
            {
                ProblemDetails problemDetails = new()
                {
                    Title = "Mixable Tracks",
                    Detail = "A minimum of two tracks are required for mixable comparison."
                };
                return TypedResults.BadRequest(problemDetails);
            }
            IEnumerable<MixableTrackResponse> response = _businessLogic.AreMixableTracks(tracks);
            return Results.Ok(response);
        })
        .MapToApiVersion(2)
        .WithTags(MicroserviceTags.AllMusic)
        .WithOpenApi()
        .Produces<IEnumerable<MixableTrackResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json);
    }

    #endregion
}