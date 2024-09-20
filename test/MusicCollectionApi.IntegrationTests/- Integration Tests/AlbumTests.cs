using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace MusicCollectionApi.IntegrationTests;

/// <summary>
/// 
/// </summary>
public sealed class AlbumTests : IntegrationTestBase
{ 
    #region Constructor and Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="testServer"></param>
    public AlbumTests(TestServer testServer) : base(testServer)
    {
    }

    #endregion

    #region Integration Test Declarations - GET

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAlbums_ReturnsOk()
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"/api/v1/albums");
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAlbumById_Exists_ReturnsOk()
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"/api/v1/albums/1");
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAlbumById_NotExists_ReturnsNotFound()
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"/api/v1/albums/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}