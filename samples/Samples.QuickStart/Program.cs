using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Logging;
using Cognas.ApiTools.Microservices;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;

namespace Samples.QuickStart;

/// <summary>
/// 
/// </summary>
public sealed class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// A (non-working!) sample of the minimum needed to get an API running. Please note this example does not include
    /// any actual data retreival or health-check endpoints.
    /// </remarks>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.ConfigureLogging(LoggingType.File);
        webApplicationBuilder.Services.AddData();
        webApplicationBuilder.Services.AddDefaultServices();
        webApplicationBuilder.Services.AddHealthChecks();
        webApplicationBuilder.Services.AddHttpClientServices();
        webApplicationBuilder.Services.AddPagination();
        webApplicationBuilder.Services.AddVersioning();
        webApplicationBuilder.Services.ConfigureSwaggerGen();
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();

        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(ICommandMicroserviceBusinessLogic<,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(webApplicationBuilder.Services, typeof(IQueryMicroserviceBusinessLogic<>), ServiceLifetime.Singleton);

        WebApplication webApplication = webApplicationBuilder.Build();
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();
        webApplication.AddSwagger();
        webApplication.ConfigureAndRun();
    }
}