using Cognas.ApiTools.SourceGenerators.CommandScaffold.Names;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators.CommandScaffold;

/// <summary>
/// 
/// </summary>
internal static class CommandMappingServiceGenerator
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="detail"></param>
    public static void Generate(SourceProductionContext context, string fullModelName, CommandScaffoldDetail detail)
    {
        string template = TemplateCache.GetTemplate(TemplateNames.CommandMappingService);
        string propertyMaps = BuildPropertyMaps(detail);
        string commandMappingServiceSource = string.Format(template,
                                                           fullModelName,
                                                           detail.RequestName,
                                                           detail.ModelNamespace,
                                                           detail.ModelName,
                                                           propertyMaps);
        string filename = $"{detail.ModelName}.{SourceFileNames.CommandMappingService}";
        context.AddSource(filename, commandMappingServiceSource);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="detail"></param>
    /// <returns></returns>
    private static string BuildPropertyMaps(CommandScaffoldDetail detail)
    {
        StringBuilder propertyMapsBuilder = new();
        propertyMapsBuilder.AppendTab(3);
        propertyMapsBuilder.Append(detail.IdPropertyName);
        propertyMapsBuilder.Append(" = request.");
        propertyMapsBuilder.Append(detail.IdPropertyName);
        propertyMapsBuilder.AppendLine(" ?? NotInsertedId,");

        foreach (string propertyName in detail.PropertyNames)
        {
            propertyMapsBuilder.AppendTab(3);
            propertyMapsBuilder.Append(propertyName);
            propertyMapsBuilder.Append(" = request.");
            propertyMapsBuilder.Append(propertyName);
            propertyMapsBuilder.AppendLine(",");
        }

        string propertyMaps = propertyMapsBuilder.ToString();
        return propertyMaps;
    }

    #endregion
}