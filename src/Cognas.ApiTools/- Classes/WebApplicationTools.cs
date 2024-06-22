using Asp.Versioning;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Data;
using Cognas.ApiTools.Data.Command;
using Cognas.ApiTools.Data.Query;
using Cognas.ApiTools.ExceptionHandling;
using Cognas.ApiTools.HealthChecks;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Messaging;
using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Shared.Extensions;
using HealthChecks.UI.Client;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Cognas.ApiTools;

/// <summary>
/// 
/// </summary>
/// <example>
[ExcludeFromCodeCoverage]
public sealed class WebApplicationTools
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public WebApplicationBuilder WebApplicationBuilder { get; }

    /// <summary>
    /// 
    /// </summary>
    public IServiceCollection ServiceCollection => WebApplicationBuilder.Services;

    /// <summary>
    /// 
    /// </summary>
    public ConfigurationManager ConfigurationManager => WebApplicationBuilder.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public IHostBuilder HostBuilder => WebApplicationBuilder.Host;

    /// <summary>
    /// 
    /// </summary>
    public ILoggingBuilder LoggingBuilder => WebApplicationBuilder.Logging;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="WebApplicationTools"/>
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    public WebApplicationTools(WebApplicationBuilder webApplicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder));
        WebApplicationBuilder = webApplicationBuilder;
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    public void AddDefaultServices()
    {
        ServiceCollection.AddAuthorization();
        ServiceCollection.AddEndpointsApiExplorer();
        ServiceCollection.AddHttpContextAccessor();
        ServiceCollection.AddMemoryCache();
        ServiceCollection.AddProblemDetails();

        GenericServiceRegistration.Instance.AddServices(ServiceCollection, typeof(ICommandApi<,,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(ServiceCollection, typeof(ICommandBusinessLogic<>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(ServiceCollection, typeof(ICommandMappingService<,,>), ServiceLifetime.Singleton);

        GenericServiceRegistration.Instance.AddServices(ServiceCollection, typeof(IQueryApi<,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(ServiceCollection, typeof(IQueryBusinessLogic<>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(ServiceCollection, typeof(IQueryMappingService<,>), ServiceLifetime.Singleton);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddApplicationInsights() => ServiceCollection.AddApplicationInsightsTelemetry();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reloadIntervalMinutes"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void AddAzureKeyVault(int reloadIntervalMinutes = 15)
    {
        string vaultUriString = ConfigurationManager.GetValue<string>("KeyVaultConfiguration:KeyVaultUri") ?? throw new NullReferenceException("KeyVaultUri");
        Uri vaultUri = new(vaultUriString);
        DefaultAzureCredential credential = new();
        AzureKeyVaultConfigurationOptions options = new() { ReloadInterval = TimeSpan.FromMinutes(reloadIntervalMinutes) };
        ConfigurationManager.AddAzureKeyVault(vaultUri, credential, options);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddExceptionHandlers()
    {
        ServiceCollection.AddExceptionHandler<PaginationQueryParametersExceptionHandler>();
        ServiceCollection.AddExceptionHandler<OperationCanceledExceptionHandler>();
        ServiceCollection.AddExceptionHandler<MapDtoToModelNotSupportedExceptionHandler>();
        ServiceCollection.AddExceptionHandler<SqlExceptionHandler>();
        ServiceCollection.AddExceptionHandler<GlobalExceptionHandler>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddData()
    {
        ServiceCollection.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
        ServiceCollection.AddSingleton<IDatabaseTransactionService, DatabaseTransactionService>();
        ServiceCollection.AddSingleton<IDynamicParameterFactory, DynamicParameterFactory>();
        ServiceCollection.AddSingleton<IIdsParameterFactory, IdsParameterFactory>();
        ServiceCollection.AddSingleton<ICommandDatabaseService, CommandDatabaseService>();
        ServiceCollection.AddSingleton<IQueryDatabaseService, QueryDatabaseService>();
    }

    /// <summary>
    /// 
    /// </summary>
    public IHealthChecksBuilder AddHealthChecks()
    {
        ServiceCollection.AddSingleton<IHealthCheckResultHelper, HealthCheckResultHelper>();
        IHealthChecksBuilder healthChecksBuilder = ServiceCollection.AddHealthChecks()
                                                                    .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck))
                                                                    .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck));
        return healthChecksBuilder;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddHttpClient()
    {
        ServiceCollection.AddHttpClient();
        ServiceCollection.AddSingleton<IHttpClientService, HttpClientService>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddPagination() => ServiceCollection.AddSingleton<IPaginationFunctions, PaginationFunctions>();

    /// <summary>
    /// 
    /// </summary>
    public void AddSignalR()
    {
        ServiceCollection.AddSignalR();
        GenericServiceRegistration.Instance.AddServices(ServiceCollection, typeof(IModelMessagingService<>), ServiceLifetime.Singleton);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TBind"></typeparam>
    public void BindConfigurationSection<TBind>() where TBind : class => ServiceCollection.Configure<TBind>(ConfigurationManager.GetSection(typeof(TBind).Name));

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void ConfigureApplicationInsightsLogging()
    {
        string connectionString = ConfigurationManager.GetValue<string>("ApplicationInsights:ConnectionString") ?? throw new NullReferenceException("ApplicationInsights");
        TelemetryConfiguration telemetryConfiguration = new() { ConnectionString = connectionString };
        LoggingBuilder.ClearProviders();
        LoggingBuilder.AddAzureWebAppDiagnostics();
        HostBuilder.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
                                                                                               .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    public void ConfigureLocalLogging(string path = "log-.log")
    {
        Serilog.ILogger logger = new LoggerConfiguration()
                                    .WriteTo.File(path, rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                                    .Enrich.FromLogContext().CreateLogger();
        LoggingBuilder.ClearProviders();
        LoggingBuilder.AddSerilog(logger);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    public static void ConfigureSwagger(WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplication"></param>
    /// <param name="healthCheckEndpoint"></param>
    public static void ConfigureAndRun(WebApplication webApplication, string healthCheckEndpoint = "/health")
    {
        webApplication.UseAuthorization();
        webApplication.UseExceptionHandler();
        webApplication.UseHttpsRedirection();
        webApplication.MapHealthChecks(healthCheckEndpoint, new()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        webApplication.Run();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void ConfigureSwaggerGen()
    {
        Assembly currentAssembly = Assembly.GetCallingAssembly();
        IEnumerable<string> xmlDocumentPaths = from assembly in currentAssembly.GetReferencedAssemblies().Union([currentAssembly.GetName()])
                                               let assemblyName = assembly.Name ?? throw new NullReferenceException(nameof(AssemblyName))
                                               let xmlDocumentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.xml")
                                               where File.Exists(xmlDocumentPath)
                                               select xmlDocumentPath;

        ServiceCollection.AddSwaggerGen(swaggerGenAction =>
        {
            swaggerGenAction.DescribeAllParametersInCamelCase();
            xmlDocumentPaths.FastForEach(xmlDocumentPath => swaggerGenAction.IncludeXmlComments(xmlDocumentPath));
        });
    }

    /// <summary>
    /// 
    /// </summary>
    public void ConfigureVersioning()
    {
        ServiceCollection.AddApiVersioning(apiVersioningAction =>
        {
            const string xApiVersion = "x-api-version";
            apiVersioningAction.DefaultApiVersion = new ApiVersion(1, 0);
            apiVersioningAction.AssumeDefaultVersionWhenUnspecified = true;
            apiVersioningAction.ReportApiVersions = true;
            apiVersioningAction.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                            new HeaderApiVersionReader(xApiVersion),
                                                                            new MediaTypeApiVersionReader(xApiVersion));
        });
    }

    #endregion
}