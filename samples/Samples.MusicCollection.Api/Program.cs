using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;
using Samples.MusicCollection.Api.AllMusic.BusinessLogic;
using Samples.MusicCollection.Api.AllMusic.Endpoints;
using Samples.MusicCollection.Api.AllMusic.TrackRules;

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

        MultipleServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IMixableTracksRule), ServiceLifetime.Singleton);

        //API gateway - simulated calls to microservices
        webApplicationBuilder.Services.AddSingleton<IAlbumMicroserviceBusinessLogic, AlbumMicroserviceBusinessLogic>();
        webApplicationBuilder.Services.AddSingleton<IAlbumEndpoints, AlbumEndpoints>();

        WebApplication webApplication = webApplicationBuilder.Build();

        //Initiate Command & Query Endpoints auto-generated via SourceGeneration
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();

        RouteGroupBuilder apiVersionRouteV3 = webApplication.GetApiVersionRoute(3);
        IAlbumEndpoints albumEndpoints = webApplication.Services.GetService<IAlbumEndpoints>() ?? throw new NullReferenceException(nameof(AlbumEndpoints));
        albumEndpoints.MapGet(apiVersionRouteV3);
        albumEndpoints.MapGetById(apiVersionRouteV3);
        albumEndpoints.MapPost(apiVersionRouteV3);
        albumEndpoints.MapPut(apiVersionRouteV3);
        albumEndpoints.MapDelete(apiVersionRouteV3);

        webApplication.AddSwagger();
        webApplication.ConfigureAndRun();
    }

    #endregion
}