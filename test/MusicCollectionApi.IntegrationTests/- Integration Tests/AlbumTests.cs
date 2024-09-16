using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;


namespace MusicCollectionApi.IntegrationTests;

/// <summary>
/// 
/// </summary>
public sealed class AlbumTests : IClassFixture<TestServer>
{
    #region Field Declarations

    private readonly HttpClient _httpClient;

    #endregion

    #region Constructor and Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factory"></param>
    public AlbumTests(TestServer factory)
    {
        IServiceScope scope = factory.Services.CreateScope();
        _httpClient = factory.CreateClient();
    }

    #endregion

    #region Integration Test Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAlbums_ReturnsOk()
    {
        throw new NotImplementedException();
    }

    #endregion
}