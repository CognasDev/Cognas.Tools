using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;
using Samples.MusicCollection.Api.Albums;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.AllMusic.Endpoints;
using Samples.MusicCollection.Api.AllMusic.TrackRules;
using Samples.MusicCollection.Api.Artists;
using Samples.MusicCollection.Api.Config;

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
        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IMicroserviceBusinessLogic<,>), ServiceLifetime.Singleton);

        //API gateway - simulated microservices
        webApplicationBuilder.Services.AddKeyedSingleton<IMicroserviceEndpoints, AlbumMicroserviceEndpoints>(nameof(Album));
        webApplicationBuilder.Services.AddKeyedSingleton<IMicroserviceEndpoints, ArtistMicroserviceEndpoints>(nameof(Artist));

        WebApplication webApplication = webApplicationBuilder.Build();

        //Initiate Command & Query Endpoints auto-generated via SourceGeneration
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();

        RouteGroupBuilder apiVersionRouteV3 = webApplication.GetApiVersionRoute(3);
        MapAlbumMicroserviceEndpoints(webApplication, apiVersionRouteV3);
        MapArtistMicroserviceEndpoints(webApplication, apiVersionRouteV3);

        webApplication.AddSwagger();
        webApplication.ConfigureAndRun();
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="apiVersionRouteV3"></param>
    /// <exception cref="NullReferenceException"></exception>
    private static void MapAlbumMicroserviceEndpoints(WebApplication webApplication, RouteGroupBuilder apiVersionRouteV3)
    {
        IMicroserviceEndpoints albumEndpoints = webApplication.Services.GetKeyedService<IMicroserviceEndpoints>(nameof(Album)) ?? throw new NullReferenceException(nameof(AlbumMicroserviceEndpoints));
        albumEndpoints.MapGet(apiVersionRouteV3);
        albumEndpoints.MapGetById(apiVersionRouteV3);
        albumEndpoints.MapPost(apiVersionRouteV3);
        albumEndpoints.MapPut(apiVersionRouteV3);
        albumEndpoints.MapDelete(apiVersionRouteV3);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="apiVersionRouteV3"></param>
    /// <exception cref="NullReferenceException"></exception>
    private static void MapArtistMicroserviceEndpoints(WebApplication webApplication, RouteGroupBuilder apiVersionRouteV3)
    {
        IMicroserviceEndpoints artistEndpoints = webApplication.Services.GetKeyedService<IMicroserviceEndpoints>(nameof(Artist)) ?? throw new NullReferenceException(nameof(ArtistMicroserviceEndpoints));
        artistEndpoints.MapGet(apiVersionRouteV3);
        artistEndpoints.MapGetById(apiVersionRouteV3);
        artistEndpoints.MapPost(apiVersionRouteV3);
        artistEndpoints.MapPut(apiVersionRouteV3);
        artistEndpoints.MapDelete(apiVersionRouteV3);
    }

    #endregion
}