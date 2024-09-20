using Cognas.ApiTools.BusinessLogic;
using Cognas.ApiTools.Mapping;
using Cognas.ApiTools.Microservices;
using Cognas.ApiTools.MinimalApi;
using Cognas.ApiTools.ServiceRegistration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Samples.MusicCollection.Api.AllMusic.MixableTracks.Rules;
using System.Reflection;
using Program = Samples.MusicCollection.Api.Program;

namespace MusicCollectionApi.IntegrationTests;

/// <summary>
/// 
/// </summary>
public sealed class TestServer : WebApplicationFactory<Program>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="TestServer"/>
    /// </summary>
    public TestServer()
    {
    }

    #endregion

    #region Protected Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            Assembly apiAssembly = typeof(Program).Assembly;
            MultipleServiceRegistration.Instance.AddServices(services, typeof(IMixableTracksRule), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(ICommandMicroserviceBusinessLogic<,>), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(IQueryMicroserviceBusinessLogic<>), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(ICommandApi<,,>), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(ICommandBusinessLogic<>), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(ICommandMappingService<,>), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(IQueryApi<,>), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(IQueryBusinessLogic<>), ServiceLifetime.Singleton, apiAssembly);
            GenericServiceRegistration.Instance.AddServices(services, typeof(IQueryMappingService<,>), ServiceLifetime.Singleton, apiAssembly);

            services.RemoveAll<IHttpClientFactory>();
            services.AddSingleton<IHttpClientFactory>(new TestHttpClientFactory(this));
        });
    }

    #endregion
}