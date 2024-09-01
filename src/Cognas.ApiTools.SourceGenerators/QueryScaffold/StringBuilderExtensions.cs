using Cognas.ApiTools.SourceGenerators.CommandScaffold;
using System.Collections.Generic;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators.QueryScaffold;

/// <summary>
/// 
/// </summary>
internal static class StringBuilderExtensions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stringBuilder"></param>
    /// <param name="detail"></param>
    /// <param name="apiVersionRepsitory"></param>
    public static void GenerateInitiateQueryEndpoints(this StringBuilder stringBuilder, QueryScaffoldDetail detail, ApiVersionRepsitory apiVersionRepsitory)
    {
        if (apiVersionRepsitory.TryAdd(detail.ApiVersion))
        {
            stringBuilder.AppendApiVersionRoute(detail.ApiVersion);
        }
        stringBuilder.Append("\t\twebApplication.InitiateApi<");
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.Append(", ");
        stringBuilder.Append(detail.ResponseName);
        stringBuilder.Append(">(");
        stringBuilder.Append(detail.ApiVersion);
        stringBuilder.Append(", apiVersionRouteV");
        stringBuilder.Append(detail.ApiVersion);
        stringBuilder.AppendLine(");");
    }

    #endregion
}