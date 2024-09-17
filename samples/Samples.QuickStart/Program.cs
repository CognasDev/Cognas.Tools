using Cognas.ApiTools.Configuration;
using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Logging;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;
using Cognas.ApiTools.Swagger;

namespace Samples.QuickStart;

/// <summary>
/// A (non-functional - compiles and runs, but calling endpoints will return 500 due to lack of business logic)
/// sample of the minimum needed to get an API running.
/// 
/// The work to get the API functioning is done via the attributes on <see cref="Example.ExampleModel"/> as well
/// as the service initation code below.
/// 
/// Please note, this example does not include any actual data retreival or health-check endpoints.
/// </summary>
public sealed class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.Configuration.ConfigureAppSettings(webApplicationBuilder.Environment);
        webApplicationBuilder.Services.AddRequiredServices();
        webApplicationBuilder.Services.AddSingleton<IModelIdService, ModelIdService>();
        webApplicationBuilder.ConfigureLogging(LoggingType.File);

        WebApplication webApplication = webApplicationBuilder.Build();
        webApplication.InitiateCommandEndpoints();
        webApplication.InitiateQueryEndpoints();
        webApplication.AddSwagger();
        webApplication.ConfigureAndRun();
    }
}