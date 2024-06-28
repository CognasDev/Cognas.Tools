using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Base;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Keys;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

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
    /// <param name="routes"></param>
    /// <returns></returns>
    public override string Route(AllMusicRoutes routes) => routes.Keys;

    #endregion
}