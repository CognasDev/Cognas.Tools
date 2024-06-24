using Asp.Versioning;
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
using Cognas.ApiTools.Services;
using Cognas.ApiTools.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Cognas.ApiTools.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddData(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
        serviceCollection.AddSingleton<IDatabaseTransactionService, DatabaseTransactionService>();
        serviceCollection.AddSingleton<IDynamicParameterFactory, DynamicParameterFactory>();
        serviceCollection.AddSingleton<IIdsParameterFactory, IdsParameterFactory>();
        serviceCollection.AddSingleton<ICommandDatabaseService, CommandDatabaseService>();
        serviceCollection.AddSingleton<IQueryDatabaseService, QueryDatabaseService>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IHealthCheckResultHelper, HealthCheckResultHelper>();
        IHealthChecksBuilder healthChecksBuilder = serviceCollection.AddHealthChecks()
                                                                    .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck))
                                                                    .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck));
        return healthChecksBuilder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddDefaultServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthorization();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddMemoryCache();
        serviceCollection.AddProblemDetails();

        serviceCollection.ConfigureOptions<ConfigureSwaggerGenOptions>();

        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(ICommandApi<,,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(ICommandBusinessLogic<>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(ICommandMappingService<,,>), ServiceLifetime.Singleton);

        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IQueryApi<,>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IQueryBusinessLogic<>), ServiceLifetime.Singleton);
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IQueryMappingService<,>), ServiceLifetime.Singleton);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddExceptionHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddExceptionHandler<PaginationQueryParametersExceptionHandler>();
        serviceCollection.AddExceptionHandler<OperationCanceledExceptionHandler>();
        serviceCollection.AddExceptionHandler<SqlExceptionHandler>();
        serviceCollection.AddExceptionHandler<GlobalExceptionHandler>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddHttpClientServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton<IHttpClientService, HttpClientService>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddPagination(this IServiceCollection serviceCollection) => serviceCollection.AddSingleton<IPaginationFunctions, PaginationFunctions>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddSignalRServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSignalR();
        GenericServiceRegistration.Instance.AddServices(serviceCollection, typeof(IModelMessagingService<>), ServiceLifetime.Singleton);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddVersioning(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(apiVersioningAction =>
        {
            const string xApiVersion = "x-api-version";
            apiVersioningAction.DefaultApiVersion = new ApiVersion(1);
            apiVersioningAction.AssumeDefaultVersionWhenUnspecified = true;
            apiVersioningAction.ReportApiVersions = true;
            apiVersioningAction.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                            new HeaderApiVersionReader(xApiVersion),
                                                                            new MediaTypeApiVersionReader(xApiVersion));
        })
        .AddApiExplorer(apiExplorerOptions =>
        {
            apiExplorerOptions.GroupNameFormat = "'v'V";
            apiExplorerOptions.SubstituteApiVersionInUrl = true;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void ConfigureSwaggerGen(this IServiceCollection serviceCollection)
    {
        Assembly currentAssembly = Assembly.GetCallingAssembly();
        IEnumerable<string> xmlDocumentPaths = from assembly in currentAssembly.GetReferencedAssemblies().Union([currentAssembly.GetName()])
                                               let assemblyName = assembly.Name ?? throw new NullReferenceException(nameof(AssemblyName))
                                               let xmlDocumentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.xml")
                                               where File.Exists(xmlDocumentPath)
                                               select xmlDocumentPath;

        serviceCollection.AddSwaggerGen(swaggerGenAction =>
        {
            swaggerGenAction.DescribeAllParametersInCamelCase();
            xmlDocumentPaths.FastForEach(xmlDocumentPath => swaggerGenAction.IncludeXmlComments(xmlDocumentPath));
        });
    }

    #endregion
}