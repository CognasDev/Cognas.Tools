using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Labels;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class LabelsMicroserviceEndpoints : MicroserviceEndpointsBase<LabelRequest, LabelResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="LabelsMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="businessLogic"></param>
    /// <param name="routes"></param>
    public LabelsMicroserviceEndpoints(IMicroserviceBusinessLogic<LabelRequest, LabelResponse> businessLogic,
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
    public override string Route(AllMusicRoutes routes) => routes.Label;

    #endregion
}