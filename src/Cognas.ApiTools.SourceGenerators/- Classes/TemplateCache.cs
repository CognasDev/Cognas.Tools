using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cognas.ApiTools.SourceGenerators;

/// <summary>
/// 
/// </summary>
internal static class TemplateCache
{
    #region Field Declarations

    private static readonly Dictionary<string, string> _cache = [];

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="templateName"></param>
    /// <returns></returns>
    public static string GetTemplate(string templateName)
    {
        if (!_cache.ContainsKey(templateName))
        {
            string resource = ReadResourceTemplateAsync(templateName).Result;
            _cache.Add(templateName, resource);
        }
        return _cache[templateName];
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="templateName"></param>
    /// <returns></returns>
    private static async Task<string> ReadResourceTemplateAsync(string templateName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourcePath = assembly.GetManifestResourceNames().Single(name => name.EndsWith(templateName));
        using Stream stream = assembly.GetManifestResourceStream(resourcePath);
        using StreamReader streamReader = new(stream);
        string resource = await streamReader.ReadToEndAsync().ConfigureAwait(false);
        return resource;
    }

    #endregion
}