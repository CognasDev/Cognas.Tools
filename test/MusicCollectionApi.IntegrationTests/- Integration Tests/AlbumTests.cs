using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Program = Samples.MusicCollection.Api.Program;

namespace MusicCollectionApi.IntegrationTests;

/// <summary>
/// 
/// </summary>
public sealed class AlbumTests : IClassFixture<WebApplicationFactory<Program>>
{
    #region Field Declarations

    private readonly HttpClient _httpClient;

    #endregion

    #region Constructor and Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factory"></param>
    public AlbumTests(WebApplicationFactory<Program> factory)
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