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

        webApplicationTools.ConfigureLocalLogging();
        webApplicationTools.ConfigureSwaggerGen();
        webApplicationTools.ConfigureVersioning();
        webApplicationTools.BindConfigurationSection<MicroserviceUris>();

        //ModelIdService is auto-generated via SourceGeneration
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();

        WebApplication webApplication = webApplicationBuilder.Build();
        WebApplicationTools.ConfigureSwagger(webApplication);

        //Initiate minimal Api endpoints
        webApplication.InitiateApi<Album, AlbumRequest, AlbumResponse>();
        webApplication.InitiateApi<Genre, GenreRequest, GenreResponse>();
        webApplication.InitiateApi<Key, KeyResponse>();
        webApplication.InitiateApi<Label, LabelRequest, LabelResponse>();

        WebApplicationTools.ConfigureAndRun(webApplication);
    }

    #endregion

}