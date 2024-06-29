using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Base;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class AlbumsMicroserviceEndpoints : CommandQueryMicroserviceEndpointsBase<AlbumRequest, AlbumResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumsMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="commandBusinessLogic"></param>
    /// <param name="queryBusinessLogic"></param>
    /// <param name="routes"></param>
    public AlbumsMicroserviceEndpoints(ICommandQueryMicroserviceBusinessLogic<AlbumRequest, AlbumResponse> commandBusinessLogic,
                                       IQueryMicroserviceBusinessLogic<AlbumResponse> queryBusinessLogic,
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
    public override string GetRoute(AllMusicRoutes allMusicRoutes) => allMusicRoutes.Albums;

    #endregion
}