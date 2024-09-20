using Microsoft.AspNetCore.Mvc.Testing;
using Samples.MusicCollection.Api;

namespace MusicCollectionApi.IntegrationTests;

/// <summary>
/// 
/// </summary>
public sealed class TestHttpClientFactory : IHttpClientFactory
{
    #region Field Declarations

    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TestHttpClientFactory"/>
    /// </summary>
    /// <param name="webApplicationFactory"></param>
    public TestHttpClientFactory(WebApplicationFactory<Program> webApplicationFactory)
    {
        ArgumentNullException.ThrowIfNull(webApplicationFactory, nameof(webApplicationFactory));
        _webApplicationFactory = webApplicationFactory;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public HttpClient CreateClient(string name) => _webApplicationFactory.CreateClient();

    #endregion
}