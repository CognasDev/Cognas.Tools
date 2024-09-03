using Cognas.ApiTools.SourceGenerators.QueryScaffold.Names;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators.QueryScaffold;

/// <summary>
/// 
/// </summary>
internal static class QueryMappingServiceGenerator
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="detail"></param>
    public static void Generate(SourceProductionContext context, string fullModelName, QueryScaffoldDetail detail)
    {
        string template = TemplateCache.GetTemplate(TemplateNames.QueryMappingService);
        string propertyMaps = BuildPropertyMaps(detail);
        string queryMappingServiceSource = string.Format(template,
                                                         fullModelName,
                                                         detail.ResponseName,
                                                         detail.ModelNamespace,
                                                         detail.ApiVersion,
                                                         detail.ModelName,
                                                         propertyMaps);
        string versionFilename = string.Format(SourceFileNames.QueryMappingService, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, queryMappingServiceSource);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="detail"></param>
    /// <returns></returns>
    private static string BuildPropertyMaps(QueryScaffoldDetail detail)
    {
        StringBuilder propertyMapsBuilder = new();
        propertyMapsBuilder.AppendTab(3);
        propertyMapsBuilder.Append(detail.IdPropertyName);
        propertyMapsBuilder.Append(" = model.");
        propertyMapsBuilder.Append(detail.IdPropertyName);
        propertyMapsBuilder.AppendLine(",");

        foreach (string propertyName in detail.PropertyNames)
        {
            propertyMapsBuilder.AppendTab(3);
            propertyMapsBuilder.Append(propertyName);
            propertyMapsBuilder.Append(" = model.");
            propertyMapsBuilder.Append(propertyName);
            propertyMapsBuilder.AppendLine(",");
        }

        string propertyMaps = propertyMapsBuilder.ToString();
        return propertyMaps;
    }

    #endregion
}