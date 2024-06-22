using Cognas.ApiTools;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;
using Samples.MusicCollection.Api.Albums;

namespace Samples.MusicCollection.Api;

/// <summary>
/// 
/// </summary>
public sealed class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
        WebApplicationTools webApplicationTools = new(webApplicationBuilder);

        webApplicationTools.AddApplicationInsights();
        webApplicationTools.AddData();
        webApplicationTools.AddDefaultServices();
        webApplicationTools.AddExceptionHandlers();
        webApplicationTools.AddHealthChecks();
        webApplicationTools.AddHttpClient();
        webApplicationTools.AddPagination();
        webApplicationTools.AddSignalR();

        webApplicationTools.ConfigureLocalLogging();
        webApplicationTools.ConfigureSwaggerGen();
        webApplicationTools.ConfigureVersioning();
        webApplicationTools.BindConfigurationSection<MicroserviceUris>();

        //ModelIdService is auto-generated via SourceGeneration
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();

        WebApplication webApplication = webApplicationBuilder.Build();
        WebApplicationTools.ConfigureSwagger(webApplication);

        //Map minimal API endpoints
        ICommandApi<Album, AlbumRequest, AlbumResponse> albumCommandApi = webApplication.Services.GetCommandApi<Album, AlbumRequest, AlbumResponse>();
        IQueryApi<Album, AlbumResponse> albumQueryApi = webApplication.Services.GetQueryApi<Album, AlbumResponse>();

        albumCommandApi.MapAll(webApplication);
        albumQueryApi.MapAll(webApplication);

        WebApplicationTools.ConfigureAndRun(webApplication);
    }
}