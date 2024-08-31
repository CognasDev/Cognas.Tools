using Cognas.ApiTools.Microservices;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Keys;

namespace Samples.MusicCollection.Api.AllMusic.Keys;

/// <summary>
/// 
/// </summary>
public sealed class KeysMicroserviceEndpoints : QueryMicroserviceEndpointsBase<KeyResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="KeysMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="queryBusinessLogic"></param>
    /// <param name="routes"></param>
    public KeysMicroserviceEndpoints(IQueryMicroserviceBusinessLogic<KeyResponse> queryBusinessLogic,
                                     IOptions<AllMusicRoutes> routes)
        : base(queryBusinessLogic, routes)
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="allMusicRoutes"></param>
    /// <returns></returns>
    public override string GetRoute(AllMusicRoutes allMusicRoutes) => allMusicRoutes.Keys;

    #endregion
}