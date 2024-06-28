using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.Abstractions;
using Samples.MusicCollection.Api.AllMusic.Endpoints;
using Samples.MusicCollection.Api.AllMusic.TrackRules;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Config;
using Samples.MusicCollection.Api.Genres;
using Samples.MusicCollection.Api.Keys;
using Samples.MusicCollection.Api.Labels;
using Samples.MusicCollection.Api.Tracks;

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

        webApplicationBuilder.BindConfigurationSection<AllMusicRoutes>();
        webApplicationBuilder.BindConfigurationSection<MicroserviceUris>();

        //ModelIdService is auto-generated via SourceGeneration
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();

        MultipleServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IMixableTracksRule), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(ICommandQueryMicroserviceBusinessLogic<,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IQueryMicroserviceBusinessLogic<>), ServiceLifetime.Singleton);

        //API gateway - simulated microservices
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandQueryMicroserviceEndpoints, AlbumsMicroserviceEndpoints>(nameof(Album));
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandQueryMicroserviceEndpoints, ArtistsMicroserviceEndpoints>(nameof(Artist));
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandQueryMicroserviceEndpoints, GenresMicroserviceEndpoints>(nameof(Genre));
        webApplicationBuilder.Services.AddKeyedSingleton<IQueryMicroserviceEndpoints, KeysMicroserviceEndpoints>(nameof(Key));
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandQueryMicroserviceEndpoints, LabelsMicroserviceEndpoints>(nameof(Label));
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandQueryMicroserviceEndpoints, TracksMicroserviceEndpoints>(nameof(Track));

        WebApplication webApplication = webApplicationBuilder.Build();

        //Initiate Command & Query Endpoints auto-generated via SourceGeneration
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();

        RouteGroupBuilder apiVersionRouteV2 = webApplication.GetApiVersionRoute(2);
        MapCommandQueryEndpoints(webApplication, apiVersionRouteV2, nameof(Album));
        MapCommandQueryEndpoints(webApplication, apiVersionRouteV2, nameof(Artist));
        MapCommandQueryEndpoints(webApplication, apiVersionRouteV2, nameof(Genre));
        MapQueryEndpoints(webApplication, apiVersionRouteV2, nameof(Key));
        MapCommandQueryEndpoints(webApplication, apiVersionRouteV2, nameof(Label));
        MapCommandQueryEndpoints(webApplication, apiVersionRouteV2, nameof(Track));

        webApplication.AddSwagger();
        webApplication.ConfigureAndRun();
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="apiVersionRouteV2"></param>
    /// <param name="key"></param>
    /// <exception cref="NullReferenceException"></exception>
    private static void MapCommandQueryEndpoints(WebApplication webApplication, RouteGroupBuilder apiVersionRouteV2, string key)
    {
        ICommandQueryMicroserviceEndpoints endpoints = webApplication.Services.GetKeyedService<ICommandQueryMicroserviceEndpoints>(key) ?? throw new NullReferenceException(nameof(TracksMicroserviceEndpoints));
        endpoints.MapGet(apiVersionRouteV2);
        endpoints.MapGetById(apiVersionRouteV2);
        endpoints.MapPost(apiVersionRouteV2);
        endpoints.MapPut(apiVersionRouteV2);
        endpoints.MapDelete(apiVersionRouteV2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="apiVersionRouteV2"></param>
    /// <param name="key"></param>
    /// <exception cref="NullReferenceException"></exception>
    private static void MapQueryEndpoints(WebApplication webApplication, RouteGroupBuilder apiVersionRouteV2, string key)
    {
        IQueryMicroserviceEndpoints endpoints = webApplication.Services.GetKeyedService<IQueryMicroserviceEndpoints>(key) ?? throw new NullReferenceException(nameof(TracksMicroserviceEndpoints));
        endpoints.MapGet(apiVersionRouteV2);
        endpoints.MapGetById(apiVersionRouteV2);
    }

    #endregion
}