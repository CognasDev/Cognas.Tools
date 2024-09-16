using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MusicCollectionApi.IntegrationTests;

/// <summary>
/// 
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime, IClassFixture<TestServer>
{
    #region Field Declarations

    private readonly TestServer _testServer;
    private AsyncServiceScope _scope;

    #endregion

    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IServiceProvider ServiceProvider { get; private set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public HttpClient HttpClient { get; private set; } = null!;

    #endregion

    #region Constructor and Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="testServer"></param>
    protected IntegrationTestBase(TestServer testServer) => _testServer = testServer;

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        WebApplicationFactoryClientOptions options = new()
        {
            BaseAddress = new Uri("https://localhost/")
        };
        HttpClient = _testServer.CreateClient(options);
        _scope = _testServer.Services.CreateAsyncScope();
        ServiceProvider = _scope.ServiceProvider;
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task DisposeAsync()
    {
        HttpClient?.Dispose();
        await _scope.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}