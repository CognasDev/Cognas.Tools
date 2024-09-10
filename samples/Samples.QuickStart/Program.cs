using Cognas.ApiTools.Endpoints;
using Cognas.ApiTools.Extensions;
using Cognas.ApiTools.Logging;
using Cognas.ApiTools.Shared;
using Cognas.ApiTools.SourceGenerators;

namespace Samples.QuickStart;

/// <summary>
/// A (non-functional - compiles and runs, but calling endpoints will return 500) sample of the minimum needed to get an API running.
/// Please note this example does not include any actual data retreival or health-check endpoints.
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