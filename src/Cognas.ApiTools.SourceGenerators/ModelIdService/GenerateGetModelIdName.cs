using System.Text;

namespace Cognas.ApiTools.SourceGenerators.ModelIdService;

/// <summary>
/// 
/// </summary>
internal static class GenerateGetModelIdName
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="getModelIdNameBuilder"></param>
    /// <param name="detail"></param>
    public static void Generate(StringBuilder getModelIdNameBuilder, ModelIdServiceEntryDetail detail)
    {
        getModelIdNameBuilder.Append("\t\t\tcase Type ");
        getModelIdNameBuilder.Append(detail.CamelCaseModelName);
        getModelIdNameBuilder.Append("Type when ");
        getModelIdNameBuilder.Append(detail.CamelCaseModelName);
        getModelIdNameBuilder.Append("Type == typeof(");
        getModelIdNameBuilder.Append(detail.ModelNamespace);
        getModelIdNameBuilder.Append('.');
        getModelIdNameBuilder.Append(detail.ModelName);
        getModelIdNameBuilder.AppendLine("):");
        getModelIdNameBuilder.Append("\t\t\t\treturn nameof(");
        getModelIdNameBuilder.Append(detail.ModelNamespace);
        getModelIdNameBuilder.Append('.');
        getModelIdNameBuilder.Append(detail.ModelName);
        getModelIdNameBuilder.Append('.');
        getModelIdNameBuilder.Append(detail.IdPropertyName);
        getModelIdNameBuilder.AppendLine(");");
    }

    #endregion
}