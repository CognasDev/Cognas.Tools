using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;

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

        //Initiate Command & Query Endpoints auto-generated via SourceGeneration
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();

        webApplication.AddSwagger();
        webApplication.ConfigureAndRun();
    }

    #endregion
}