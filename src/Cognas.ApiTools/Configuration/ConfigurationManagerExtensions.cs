using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Cognas.ApiTools.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cognas.ApiTools.Configuration;

/// <summary>
/// 
/// </summary>
public static class ConfigurationManagerExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configurationManager"></param>
    /// <param name="reloadIntervalMinutes"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void AddAzureKeyVault(this IConfigurationManager configurationManager, int reloadIntervalMinutes = 15)
    {
        string vaultUriString = configurationManager.GetValue<string>("KeyVaultConfiguration:KeyVaultUri") ?? throw new NullReferenceException("KeyVaultUri");
        Uri vaultUri = new(vaultUriString);
        DefaultAzureCredential credential = new();
        AzureKeyVaultConfigurationOptions options = new() { ReloadInterval = TimeSpan.FromMinutes(reloadIntervalMinutes) };
        configurationManager.AddAzureKeyVault(vaultUri, credential, options);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TBind"></typeparam>
    /// <param name="webApplicationBuilder"></param>
    public static void BindConfigurationSection<TBind>(this WebApplicationBuilder webApplicationBuilder) where TBind : class
        => webApplicationBuilder.Services.Configure<TBind>(webApplicationBuilder.Configuration.GetSection(typeof(TBind).Name));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configurationManager"></param>
    /// <param name="environment"></param>
    public static void ConfigureAppSettings(this IConfigurationManager configurationManager, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(environment, nameof(environment));
        configurationManager.AddJsonFile("appsettings.json", false, true)
                            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true);
    }

    #endregion
}