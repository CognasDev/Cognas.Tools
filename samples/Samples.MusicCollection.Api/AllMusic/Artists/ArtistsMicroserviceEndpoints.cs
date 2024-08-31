using Cognas.ApiTools.Microservices;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Artists;

/// <summary>
/// 
/// </summary>
public sealed class ArtistsMicroserviceEndpoints : CommandQueryMicroserviceEndpointsBase<ArtistRequest, ArtistResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistsMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="commandBusinessLogic"></param>
    /// <param name="queryBusinessLogic"></param>
    /// <param name="routes"></param>
    public ArtistsMicroserviceEndpoints(ICommandMicroserviceBusinessLogic<ArtistRequest, ArtistResponse> commandBusinessLogic,
                                        IQueryMicroserviceBusinessLogic<ArtistResponse> queryBusinessLogic,
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
    public override string GetRoute(AllMusicRoutes allMusicRoutes) => allMusicRoutes.Artists;

    #endregion
}