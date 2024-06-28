using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Base;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Labels;

namespace Samples.MusicCollection.Api.AllMusic.BusinessLogic;

/// <summary>
/// 
/// </summary>
public sealed class LabelsMicroserviceBusinessLogic : CommandQueryMicroserviceBusinessLogicBase<LabelRequest, LabelResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="LabelsMicroserviceBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public LabelsMicroserviceBusinessLogic(ILogger<LabelsMicroserviceBusinessLogic> logger,
                                           IHttpClientService httpClientService,
                                           IOptionsMonitor<MicroserviceUris> microserviceUrisMonitor,
                                           IPaginationFunctions paginationFunctions)
        : base(logger, httpClientService, microserviceUrisMonitor, paginationFunctions)
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="microserviceUris"></param>
    /// <returns></returns>
    public override string MicroserviceUri(MicroserviceUris microserviceUris) => microserviceUris.Labels;

    #endregion
}