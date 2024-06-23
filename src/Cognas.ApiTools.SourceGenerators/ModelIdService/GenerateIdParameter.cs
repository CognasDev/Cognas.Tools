using System.Text;

namespace Cognas.ApiTools.SourceGenerators.ModelIdService;

/// <summary>
/// 
/// </summary>
internal static class GenerateIdParameter
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idExpressionBuilder"></param>
    /// <param name="detail"></param>
    public static void Generate(StringBuilder idExpressionBuilder, ModelIdServiceEntryDetail detail)
    {
        idExpressionBuilder.Append("\t\t\t");
        idExpressionBuilder.Append("case Type ");
        idExpressionBuilder.Append(detail.CamelCaseModelName);
        idExpressionBuilder.Append("Type when ");
        idExpressionBuilder.Append(detail.CamelCaseModelName);
        idExpressionBuilder.Append("Type == typeof(");
        idExpressionBuilder.Append(detail.ModelNamespace);
        idExpressionBuilder.Append('.');
        idExpressionBuilder.Append(detail.ModelName);
        idExpressionBuilder.AppendLine("):");
        idExpressionBuilder.Append("\t\t\t\t");
        idExpressionBuilder.Append("return new Parameter(nameof(");
        idExpressionBuilder.Append(detail.ModelNamespace);
        idExpressionBuilder.Append('.');
        idExpressionBuilder.Append(detail.ModelName);
        idExpressionBuilder.Append('.');
        idExpressionBuilder.Append(detail.IdPropertyName);
        idExpressionBuilder.AppendLine("), id);");
    }

    #endregion
}