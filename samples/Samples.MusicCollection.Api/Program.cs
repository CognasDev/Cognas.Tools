using Cognas.ApiTools;
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
        WebApplicationTools webApplicationTools = new(webApplicationBuilder);

        webApplicationTools.AddApplicationInsights();
        webApplicationTools.AddData();
        webApplicationTools.AddDefaultServices();
        webApplicationTools.AddExceptionHandlers();
        webApplicationTools.AddHealthChecks();
        webApplicationTools.AddHttpClient();
        webApplicationTools.AddPagination();
        webApplicationTools.AddSignalR();
        webApplicationTools.AddVersioning();
        webApplicationTools.ConfigureLocalLogging();
        webApplicationTools.ConfigureSwaggerGen();

        webApplicationTools.BindConfigurationSection<MicroserviceUris>();

        //ModelIdService is auto-generated via SourceGeneration
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();

        WebApplication webApplication = webApplicationBuilder.Build();
        WebApplicationTools.ConfigureSwagger(webApplication);
        RouteGroupBuilder apiVersionOneRoutes = WebApplicationTools.GetApiVersionRoute(webApplication, 1);

        //Initiate minimal Api endpoints
        webApplication.InitiateApi<Album, AlbumRequest, AlbumResponse>(apiVersionOneRoutes);
        webApplication.InitiateApi<Genre, GenreRequest, GenreResponse>(apiVersionOneRoutes);
        webApplication.InitiateApi<Key, KeyResponse>(apiVersionOneRoutes);
        webApplication.InitiateApi<Label, LabelRequest, LabelResponse>(apiVersionOneRoutes);

        WebApplicationTools.ConfigureAndRun(webApplication);
    }

    #endregion

}