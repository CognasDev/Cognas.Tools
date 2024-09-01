using System.Text;

namespace Cognas.ApiTools.SourceGenerators.ModelIdService;

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
    public static void GenerateGetId(this StringBuilder stringBuilder, ModelIdServiceEntryDetail detail)
    {
        stringBuilder.AppendTab(3);
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.Append(' ');
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append(" => ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.IdPropertyName);
        stringBuilder.AppendLine(",");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stringBuilder"></param>
    /// <param name="detail"></param>
    public static void GenerateGetModelIdName(this StringBuilder stringBuilder, ModelIdServiceEntryDetail detail)
    {
        stringBuilder.AppendTab(3);
        stringBuilder.Append("case Type ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append("Type when ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append("Type == typeof(");
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.AppendLine("):");
        stringBuilder.AppendTab(4);
        stringBuilder.Append("return nameof(");
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.IdPropertyName);
        stringBuilder.AppendLine(");");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stringBuilder"></param>
    /// <param name="detail"></param>
    public static void GenerateIdParameter(this StringBuilder stringBuilder, ModelIdServiceEntryDetail detail)
    {
        stringBuilder.AppendTab(3);
        stringBuilder.Append("case Type ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append("Type when ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append("Type == typeof(");
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.AppendLine("):");
        stringBuilder.AppendTab(4);
        stringBuilder.Append("return new Parameter(nameof(");
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.IdPropertyName);
        stringBuilder.AppendLine("), id);");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stringBuilder"></param>
    /// <param name="detail"></param>
    public static void GenerateSetIdValue(this StringBuilder stringBuilder, ModelIdServiceEntryDetail detail)
    {
        stringBuilder.AppendTab(3);
        stringBuilder.Append("case Type ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append("Type when ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append("Type == typeof(");
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.AppendLine("):");
        stringBuilder.AppendTab(4);
        stringBuilder.Append("var ");
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append(" = (model as ");
        stringBuilder.Append(detail.ModelNamespace);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.ModelName);
        stringBuilder.AppendLine(")!;");
        stringBuilder.AppendTab(4);
        stringBuilder.Append(detail.CamelCaseModelName);
        stringBuilder.Append('.');
        stringBuilder.Append(detail.IdPropertyName);
        stringBuilder.AppendLine(" = id;");
        stringBuilder.AppendTab(4);
        stringBuilder.AppendLine("break;");
    }

    #endregion
}