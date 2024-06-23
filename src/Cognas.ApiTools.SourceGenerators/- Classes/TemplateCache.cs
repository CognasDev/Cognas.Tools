using System;
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
    private static readonly Lazy<Assembly> _executingAssembly = new(() => Assembly.GetExecutingAssembly());
    private static readonly Lazy<string[]> _manifestResourceStreams = new(() => _executingAssembly.Value.GetManifestResourceNames());

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="templateName"></param>
    /// <returns></returns>
    public static string GetTemplate(string templateName)
    {
        bool containsTemplate = _cache.TryGetValue(templateName, out string template);
        if (!containsTemplate)
        {
            template = ReadResourceTemplateAsync(templateName).Result;
            _cache.Add(templateName, template);
        }
        return template;
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
        string resourcePath = _manifestResourceStreams.Value.Single(name => name.EndsWith(templateName));
        using Stream stream = _executingAssembly.Value.GetManifestResourceStream(resourcePath);
        using StreamReader streamReader = new(stream);
        string resource = await streamReader.ReadToEndAsync().ConfigureAwait(false);
        return resource;
    }

    #endregion
}