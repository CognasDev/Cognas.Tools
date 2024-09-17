using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Data;
using Cognas.ApiTools.Data.Command;
using Cognas.ApiTools.Data.Query;
using Cognas.ApiTools.ExceptionHandling;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Messaging;
using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.Pagination;
using Cognas.ApiTools.ServiceRegistration;
using Cognas.ApiTools.Services;
using Cognas.ApiTools.Swagger;
using Cognas.ApiTools.Versioning;
using Microsoft.Extensions.DependencyInjection;

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
    /// <param name="useMessaging"></param>
    public static void AddRequiredServices(this IServiceCollection serviceCollection, bool useMessaging = false)
    {
        serviceCollection.AddAuthorization();
        serviceCollection.AddData();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddExceptionHandlers();
        serviceCollection.AddHealthChecks();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddHttpClientServices();
        serviceCollection.AddMemoryCache();
        serviceCollection.AddProblemDetails();
        serviceCollection.AddVersioning();
        serviceCollection.AddSingleton<IPaginationFunctions, PaginationFunctions>();

        if (useMessaging)
        {
            serviceCollection.AddSignalRServices();
        }

        serviceCollection.AddMinimalApiServices();
        serviceCollection.ConfigureOptions<ConfigureSwaggerGenOptions>();
        serviceCollection.ConfigureSwaggerGen();
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

    #endregion
}