using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Albums;

/// <summary>
/// 
/// </summary>
public sealed class AlbumsMicroserviceBusinessLogic : CommandQueryMicroserviceBusinessLogicBase<AlbumRequest, AlbumResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="AlbumsMicroserviceBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public AlbumsMicroserviceBusinessLogic(ILogger<AlbumsMicroserviceBusinessLogic> logger,
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
    public override string MicroserviceUri(MicroserviceUris microserviceUris) => microserviceUris.Albums;

    #endregion
}