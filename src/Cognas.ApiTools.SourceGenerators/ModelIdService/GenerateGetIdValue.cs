using System.Text;

namespace Cognas.ApiTools.SourceGenerators.ModelIdService;

/// <summary>
/// 
/// </summary>
internal static class GenerateGetId
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getIdsBuilder"></param>
    /// <param name="detail"></param>
    public static void Generate(StringBuilder getIdsBuilder, ModelIdServiceEntryDetail detail)
    {
        getIdsBuilder.Append("\t\t\t");
        getIdsBuilder.Append(detail.ModelNamespace);
        getIdsBuilder.Append('.');
        getIdsBuilder.Append(detail.ModelName);
        getIdsBuilder.Append(' ');
        getIdsBuilder.Append(detail.CamelCaseModelName);
        getIdsBuilder.Append(" => ");
        getIdsBuilder.Append(detail.CamelCaseModelName);
        getIdsBuilder.Append('.');
        getIdsBuilder.Append(detail.IdPropertyName);
        getIdsBuilder.AppendLine(",");
    }

    #endregion
}