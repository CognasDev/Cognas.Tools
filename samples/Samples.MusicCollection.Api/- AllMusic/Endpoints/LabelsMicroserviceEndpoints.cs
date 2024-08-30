using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Base;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Labels;

namespace Samples.MusicCollection.Api.AllMusic.Endpoints;

/// <summary>
/// 
/// </summary>
public sealed class LabelsMicroserviceEndpoints : CommandQueryMicroserviceEndpointsBase<LabelRequest, LabelResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="LabelsMicroserviceEndpoints"/>
    /// </summary>
    /// <param name="commandBusinessLogic"></param>
    /// <param name="queryBusinessLogic"></param>
    /// <param name="routes"></param>
    public LabelsMicroserviceEndpoints(ICommandMicroserviceBusinessLogic<LabelRequest, LabelResponse> commandBusinessLogic,
                                       IQueryMicroserviceBusinessLogic<LabelResponse> queryBusinessLogic,
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
    public override string GetRoute(AllMusicRoutes allMusicRoutes) => allMusicRoutes.Labels;

    #endregion
}