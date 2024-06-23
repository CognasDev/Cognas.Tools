using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Cognas.ApiTools.Extensions;

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
    public static void AddAzureKeyVault(this ConfigurationManager configurationManager, int reloadIntervalMinutes = 15)
    {
        string vaultUriString = configurationManager.GetValue<string>("KeyVaultConfiguration:KeyVaultUri") ?? throw new NullReferenceException("KeyVaultUri");
        Uri vaultUri = new(vaultUriString);
        DefaultAzureCredential credential = new();
        AzureKeyVaultConfigurationOptions options = new() { ReloadInterval = TimeSpan.FromMinutes(reloadIntervalMinutes) };
        configurationManager.AddAzureKeyVault(vaultUri, credential, options);
    }

    #endregion
}