﻿using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Artists;

/// <summary>
/// 
/// </summary>
public sealed class ArtistsMicroserviceBusinessLogic : CommandQueryMicroserviceBusinessLogicBase<ArtistRequest, ArtistResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ArtistsMicroserviceBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public ArtistsMicroserviceBusinessLogic(ILogger<ArtistsMicroserviceBusinessLogic> logger,
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
    public override string MicroserviceUri(MicroserviceUris microserviceUris) => microserviceUris.Artists;

    #endregion
}