using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.BusinessLogic;

/// <summary>
/// 
/// </summary>
public sealed class ArtistMicroserviceBusinessLogic : MicroserviceBusinessLogicBase<ArtistRequest, ArtistResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistMicroserviceBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public ArtistMicroserviceBusinessLogic(ILogger<ArtistMicroserviceBusinessLogic> logger,
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
    public override string MicroserviceUri(MicroserviceUris microserviceUris) => microserviceUris.Artist;

    #endregion
}