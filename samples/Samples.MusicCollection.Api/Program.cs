using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.Genres;
using Samples.MusicCollection.Api.Keys;
using Samples.MusicCollection.Api.Labels;

namespace Samples.MusicCollection.Api;

/// <summary>
/// 
/// </summary>
public sealed class Program
{
    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.Logging.ConfigureLocalLogging();
        webApplicationBuilder.Services.AddApplicationInsightsTelemetry();
        webApplicationBuilder.Services.AddData();
        webApplicationBuilder.Services.AddDefaultHealthChecks();
        webApplicationBuilder.Services.AddDefaultServices();
        webApplicationBuilder.Services.AddExceptionHandlers();
        webApplicationBuilder.Services.AddHttpClientServices();
        webApplicationBuilder.Services.AddPagination();
        webApplicationBuilder.Services.AddSignalRServices();
        webApplicationBuilder.Services.AddVersioning();
        webApplicationBuilder.Services.ConfigureSwaggerGen();

        webApplicationBuilder.BindConfigurationSection<MicroserviceUris>();

        //ModelIdService is auto-generated via SourceGeneration
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();

        WebApplication webApplication = webApplicationBuilder.Build();

        webApplication.AddSwagger();
        InitiateVersionedApis(webApplication);

        webApplication.ConfigureAndRun();
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    private static void InitiateVersionedApis(WebApplication webApplication)
    {
        RouteGroupBuilder apiVersionOneRoutes = webApplication.GetApiVersionRoute(1);
        webApplication.InitiateApi<Album, AlbumRequest, AlbumResponse>(apiVersionOneRoutes);
        webApplication.InitiateApi<Genre, GenreRequest, GenreResponse>(apiVersionOneRoutes);
        webApplication.InitiateApi<Key, KeyResponse>(apiVersionOneRoutes);
        webApplication.InitiateApi<Label, LabelRequest, LabelResponse>(apiVersionOneRoutes);
    }

    #endregion

}