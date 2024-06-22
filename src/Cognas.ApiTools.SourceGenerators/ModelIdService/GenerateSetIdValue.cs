using System.Text;

namespace Cognas.ApiTools.SourceGenerators.ModelIdService;

/// <summary>
/// 
/// </summary>
internal static class GenerateSetIdValue
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="setKeyValuesBuilder"></param>
    /// <param name="detail"></param>
    public static void Generate(StringBuilder setKeyValuesBuilder, ModelIdServiceEntryDetail detail)
    {
        setKeyValuesBuilder.Append("\t\t\tcase Type ");
        setKeyValuesBuilder.Append(detail.CamelCaseModelName);
        setKeyValuesBuilder.Append("Type when ");
        setKeyValuesBuilder.Append(detail.CamelCaseModelName);
        setKeyValuesBuilder.Append("Type == typeof(");
        setKeyValuesBuilder.Append(detail.ModelNamespace);
        setKeyValuesBuilder.Append('.');
        setKeyValuesBuilder.Append(detail.ModelName);
        setKeyValuesBuilder.AppendLine("):");
        setKeyValuesBuilder.Append("\t\t\t\tvar ");
        setKeyValuesBuilder.Append(detail.CamelCaseModelName);
        setKeyValuesBuilder.Append(" = (model as ");
        setKeyValuesBuilder.Append(detail.ModelNamespace);
        setKeyValuesBuilder.Append('.');
        setKeyValuesBuilder.Append(detail.ModelName);
        setKeyValuesBuilder.AppendLine(")!;");
        setKeyValuesBuilder.Append("\t\t\t\t");
        setKeyValuesBuilder.Append(detail.CamelCaseModelName);
        setKeyValuesBuilder.Append('.');
        setKeyValuesBuilder.Append(detail.IdPropertyName);
        setKeyValuesBuilder.AppendLine(" = id;");
        setKeyValuesBuilder.Append("\t\t\t\t");
        setKeyValuesBuilder.AppendLine("break;");
    }

    #endregion
}