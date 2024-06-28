using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Tracks;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class TracksMicroserviceEndpoints : MicroserviceEndpointsBase<TrackRequest, TrackResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TracksMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="businessLogic"></param>
    /// <param name="routes"></param>
    public TracksMicroserviceEndpoints(IMicroserviceBusinessLogic<TrackRequest, TrackResponse> businessLogic,
                                       IOptions<AllMusicRoutes> routes)
        : base (businessLogic, routes)
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    public override string Route(AllMusicRoutes routes) => routes.Track;

    #endregion
}