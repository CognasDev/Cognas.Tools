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
using Samples.MusicCollection.Api.Genres;
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
        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IMicroserviceBusinessLogic<,>), ServiceLifetime.Singleton);

        //API gateway - simulated microservices
        webApplicationBuilder.Services.AddKeyedSingleton<IMicroserviceEndpoints, AlbumsMicroserviceEndpoints>(nameof(Album));
        webApplicationBuilder.Services.AddKeyedSingleton<IMicroserviceEndpoints, ArtistsMicroserviceEndpoints>(nameof(Artist));
        webApplicationBuilder.Services.AddKeyedSingleton<IMicroserviceEndpoints, GenresMicroserviceEndpoints>(nameof(Genre));
        webApplicationBuilder.Services.AddKeyedSingleton<IMicroserviceEndpoints, LabelsMicroserviceEndpoints>(nameof(Label));
        webApplicationBuilder.Services.AddKeyedSingleton<IMicroserviceEndpoints, TracksMicroserviceEndpoints>(nameof(Track));

        WebApplication webApplication = webApplicationBuilder.Build();

        //Initiate Command & Query Endpoints auto-generated via SourceGeneration
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();

        RouteGroupBuilder apiVersionRouteV2 = webApplication.GetApiVersionRoute(2);
        MapMicroserviceEndpoints<Album>(webApplication, apiVersionRouteV2);
        MapMicroserviceEndpoints<Artist>(webApplication, apiVersionRouteV2);
        MapMicroserviceEndpoints<Genre>(webApplication, apiVersionRouteV2);
        MapMicroserviceEndpoints<Label>(webApplication, apiVersionRouteV2);
        MapMicroserviceEndpoints<Track>(webApplication, apiVersionRouteV2);

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
    /// <exception cref="NullReferenceException"></exception>
    private static void MapMicroserviceEndpoints<TModel>(WebApplication webApplication, RouteGroupBuilder apiVersionRouteV2)
    {
        string key = typeof(TModel).Name;
        IMicroserviceEndpoints trackEndpoints = webApplication.Services.GetKeyedService<IMicroserviceEndpoints>(key) ?? throw new NullReferenceException(nameof(TracksMicroserviceEndpoints));
        trackEndpoints.MapGet(apiVersionRouteV2);
        trackEndpoints.MapGetById(apiVersionRouteV2);
        trackEndpoints.MapPost(apiVersionRouteV2);
        trackEndpoints.MapPut(apiVersionRouteV2);
        trackEndpoints.MapDelete(apiVersionRouteV2);
    }

    #endregion
}