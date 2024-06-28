using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class ArtistMicroserviceEndpoints : MicroserviceEndpointsBase<ArtistRequest, ArtistResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="businessLogic"></param>
    /// <param name="routesMonitor"></param>
    public ArtistMicroserviceEndpoints(IMicroserviceBusinessLogic<ArtistRequest, ArtistResponse> businessLogic,
                                       IOptionsMonitor<AllMusicRoutes> routesMonitor)
        : base(businessLogic, routesMonitor)
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    public override string Route(AllMusicRoutes routes) => routes.Artist;

    #endregion
}