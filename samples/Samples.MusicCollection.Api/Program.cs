using Cognas.ApiTools.Configuration;
using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Logging;
using Cognas.ApiTools.Microservices;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;
using Cognas.ApiTools.Swagger;
using Cognas.ApiTools.Versioning;
using Samples.MusicCollection.Api.AllMusic;
using Samples.MusicCollection.Api.AllMusic.Albums;
using Samples.MusicCollection.Api.AllMusic.Artists;
using Samples.MusicCollection.Api.AllMusic.Genres;
using Samples.MusicCollection.Api.AllMusic.Keys;
using Samples.MusicCollection.Api.AllMusic.Labels;
using Samples.MusicCollection.Api.AllMusic.MixableTracks.Rules;
using Samples.MusicCollection.Api.AllMusic.Tracks;
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

        webApplicationBuilder.Configuration.ConfigureAppSettings(webApplicationBuilder.Environment);
        webApplicationBuilder.Services.AddRequiredServices(true);
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();
        webApplicationBuilder.ConfigureLogging(LoggingType.File);

        webApplicationBuilder.BindConfigurationSection<AllMusicRoutes>();
        webApplicationBuilder.BindConfigurationSection<MicroserviceUris>();

        MultipleServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IMixableTracksRule), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(ICommandMicroserviceBusinessLogic<,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IQueryMicroserviceBusinessLogic<>), ServiceLifetime.Singleton);

        //API gateway - simulated microservices
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandMicroserviceEndpoints, AlbumsMicroserviceEndpoints>(MicroserviceDependencyKeys.Albums);
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandMicroserviceEndpoints, ArtistsMicroserviceEndpoints>(MicroserviceDependencyKeys.Artists);
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandMicroserviceEndpoints, GenresMicroserviceEndpoints>(MicroserviceDependencyKeys.Genres);
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandMicroserviceEndpoints, LabelsMicroserviceEndpoints>(MicroserviceDependencyKeys.Labels);
        webApplicationBuilder.Services.AddKeyedSingleton<ICommandMicroserviceEndpoints, TracksMicroserviceEndpoints>(MicroserviceDependencyKeys.Tracks);

        webApplicationBuilder.Services.AddKeyedSingleton<IQueryMicroserviceEndpoints, AlbumsMicroserviceEndpoints>(MicroserviceDependencyKeys.Albums);
        webApplicationBuilder.Services.AddKeyedSingleton<IQueryMicroserviceEndpoints, ArtistsMicroserviceEndpoints>(MicroserviceDependencyKeys.Artists);
        webApplicationBuilder.Services.AddKeyedSingleton<IQueryMicroserviceEndpoints, GenresMicroserviceEndpoints>(MicroserviceDependencyKeys.Genres);
        webApplicationBuilder.Services.AddKeyedSingleton<IQueryMicroserviceEndpoints, KeysMicroserviceEndpoints>(MicroserviceDependencyKeys.Keys);
        webApplicationBuilder.Services.AddKeyedSingleton<IQueryMicroserviceEndpoints, LabelsMicroserviceEndpoints>(MicroserviceDependencyKeys.Labels);
        webApplicationBuilder.Services.AddKeyedSingleton<IQueryMicroserviceEndpoints, TracksMicroserviceEndpoints>(MicroserviceDependencyKeys.Tracks);

        webApplicationBuilder.Services.AddSingleton<IAllMusicBusinessLogic, AllMusicBusinessLogic>();
        webApplicationBuilder.Services.AddSingleton<IAllMusicEndpoints, AllMusicEndpoints>();

        WebApplication webApplication = webApplicationBuilder.Build();

        //Initiate Command & Query Endpoints auto-generated via SourceGeneration
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();

        RouteGroupBuilder apiVersionRouteV2 = webApplication.GetApiVersionRoute(2);
        IAllMusicEndpoints allMusicEndpoints = webApplication.Services.GetService<IAllMusicEndpoints>() ?? throw new NullReferenceException(nameof(AllMusicEndpoints));
        allMusicEndpoints.MapGet(apiVersionRouteV2);
        allMusicEndpoints.MapPostAreMixableTracks(apiVersionRouteV2);

        webApplication.AddSwagger();
        webApplication.ConfigureAndRun();
    }

    #endregion
}