using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Base;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Tracks;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class TracksMicroserviceEndpoints : CommandQueryMicroserviceEndpointsBase<TrackRequest, TrackResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TracksMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="commandBusinessLogic"></param>
    /// <param name="queryBusinessLogic"></param>
    /// <param name="routes"></param>
    public TracksMicroserviceEndpoints(ICommandQueryMicroserviceBusinessLogic<TrackRequest, TrackResponse> commandBusinessLogic,
                                       IQueryMicroserviceBusinessLogic<TrackResponse> queryBusinessLogic,
                                       IOptions<AllMusicRoutes> routes)
        : base(commandBusinessLogic, queryBusinessLogic, routes)
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="allMusicRoutes"></param>
    /// <returns></returns>
    public override string GetRoute(AllMusicRoutes allMusicRoutes) => allMusicRoutes.Tracks;

    #endregion
}