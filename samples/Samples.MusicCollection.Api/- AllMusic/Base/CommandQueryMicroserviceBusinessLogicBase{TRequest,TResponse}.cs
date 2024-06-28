using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.Services;
using Microsoft.Extensions.Options;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.Config;

namespace Samples.MusicCollection.Api.AllMusic.Base;

/// <summary>
/// 
/// </summary>
public abstract class CommandQueryMicroserviceBusinessLogicBase<TRequest, TResponse> :
    QueryMicroserviceBusinessLogicBase<TResponse>, IDisposable, ICommandQueryMicroserviceBusinessLogic<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : class
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandQueryMicroserviceBusinessLogicBase{TRequest, TResponse}"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClientService"></param>
    /// <param name="microserviceUrisMonitor"></param>
    /// <param name="paginationFunctions"></param>
    public CommandQueryMicroserviceBusinessLogicBase(ILogger logger,
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
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TResponse?> PostAsync(TRequest request)
    {
        string requestUri = MicroserviceUri(MicroserviceUris);
        TResponse? postedResponse = await HttpClientService.PostAsync<TRequest, TResponse>(requestUri, request).ConfigureAwait(false);
        return postedResponse;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TResponse?> PutAsync(TRequest request)
    {
        string requestUri = MicroserviceUri(MicroserviceUris);
        TResponse? putResponse = await HttpClientService.PutAsync<TRequest, TResponse>(requestUri, request).ConfigureAwait(false);
        return putResponse;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        string requestUri = $"{MicroserviceUri(MicroserviceUris)}/{id}";
        await HttpClientService.DeleteAsync(requestUri).ConfigureAwait(false);
    }

    #endregion
}