using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Genres;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class GenresMicroserviceEndpoints : MicroserviceEndpointsBase<GenreRequest, GenreResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GenresMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="businessLogic"></param>
    /// <param name="routes"></param>
    public GenresMicroserviceEndpoints(IMicroserviceBusinessLogic<GenreRequest, GenreResponse> businessLogic,
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
    public override string Route(AllMusicRoutes routes) => routes.Genre;

    #endregion
}