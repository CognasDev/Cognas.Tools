﻿using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Keys;

namespace Samples.MusicCollection.Api.AllMusic.Keys;

/// <summary>
/// 
/// </summary>
public sealed class KeysMicroserviceBusinessLogic : QueryMicroserviceBusinessLogicBase<KeyResponse>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="KeysMicroserviceBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public KeysMicroserviceBusinessLogic(ILogger<KeysMicroserviceBusinessLogic> logger,
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
    public override string MicroserviceUri(MicroserviceUris microserviceUris) => microserviceUris.Keys;

    #endregion
}