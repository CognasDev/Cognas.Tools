using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators;

/// <summary>
/// 
/// </summary>
internal static class ExtensionMethods
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelDeclaration"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static string GetNamespace(this RecordDeclarationSyntax modelDeclaration)
    {
        IEnumerable<SyntaxNode> modelAncestors = modelDeclaration!.Ancestors();
        if (modelAncestors.FirstOrDefault(syntaxNode => syntaxNode is FileScopedNamespaceDeclarationSyntax) is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclaration)
        {
            return fileScopedNamespaceDeclaration.Name.ToString();
        }
        if (modelAncestors.LastOrDefault(syntaxNode => syntaxNode is NamespaceDeclarationSyntax) is NamespaceDeclarationSyntax namespaceDeclaration)
        {
            return namespaceDeclaration.Name.ToString();
        }
        throw new NullReferenceException("Namespace not found.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelDeclaration"></param>
    /// <returns></returns>
    public static string GetName(this RecordDeclarationSyntax modelDeclaration) => modelDeclaration.Identifier.Text;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="attributeData"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static TValue GetConstructorArgumentValue<TValue>(this AttributeData attributeData, int index)
    {
        TypedConstant constructorArgument = attributeData.ConstructorArguments[index];
        Type valueType = typeof(TValue);
        object? argumentValue = constructorArgument.Value;

        if (valueType == typeof(string))
        {
            string stringValue = GetStringArgumentValue(argumentValue, index);
            TValue returnValue = (TValue)Convert.ChangeType(stringValue, valueType);
            return returnValue;
        }

        return argumentValue is null ? throw new NullReferenceException($"{nameof(AttributeData.ConstructorArguments)} at {index} is null.") : (TValue)argumentValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public static string GetIdPropertyName(this RecordDeclarationSyntax record)
    {
        IEnumerable<PropertyDeclarationSyntax> properties = record.Members.Select(memberDeclarationSyntax => memberDeclarationSyntax).OfType<PropertyDeclarationSyntax>();
        ReadOnlySpan<PropertyDeclarationSyntax> span = [.. properties];
        List<string> idPropertyNames = new(span.Length);

        foreach (PropertyDeclarationSyntax? propertyDeclaration in span)
        {
            AttributeSyntax? idAttribute = propertyDeclaration!.AttributeLists
                                                               .SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
                                                               .Where(attributeSyntax => attributeSyntax.Name.ToString() == "Id")
                                                               .SingleOrDefault();
            if (idAttribute is not null)
            {
                idPropertyNames.Add(propertyDeclaration.Identifier.Text);
            }
        }
        return idPropertyNames.Count switch
        {
            1 => idPropertyNames[0],
            > 1 => throw new InvalidOperationException($"Multipled Id attributes found on model '{record.Identifier}'."),
            _ => throw new KeyNotFoundException($"Id attribute not found on model '{record.Identifier}'.")
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stringBuilder"></param>
    /// <param name="apiVersion"></param>
    public static void AppendApiVersionRoute(this StringBuilder stringBuilder, int apiVersion)
    {
        stringBuilder.AppendTab(2);
        stringBuilder.Append("RouteGroupBuilder apiVersionRouteV");
        stringBuilder.Append(apiVersion);
        stringBuilder.Append(" = webApplication.GetApiVersionRoute(");
        stringBuilder.Append(apiVersion);
        stringBuilder.AppendLine(");");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelDeclaration"></param>
    public static IList<string> GetModelProperties(this RecordDeclarationSyntax modelDeclaration)
    {
        List<string> propertyNames = modelDeclaration.Members.Select(memberDeclarationSyntax => memberDeclarationSyntax)
                                                             .OfType<PropertyDeclarationSyntax>()
                                                             .Select(propertyDeclarationSyntax => propertyDeclarationSyntax.Identifier.Text).ToList();
        return propertyNames;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="generatorSyntaxContext"></param>
    /// <returns></returns>
    public static RecordDeclarationSyntax GetModelDeclaration(this GeneratorAttributeSyntaxContext generatorSyntaxContext)
    {
        RecordDeclarationSyntax modelDeclaration = (RecordDeclarationSyntax)generatorSyntaxContext.TargetNode;
        return modelDeclaration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stringBuilder"></param>
    /// <param name="tabCount"></param>
    public static void AppendTab(this StringBuilder stringBuilder, int tabCount)
    {
        for (int tabs = 0; tabs < tabCount; tabs++)
        {
            stringBuilder.Append("\t");
        }
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="argumentValue"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    private static string GetStringArgumentValue(object? argumentValue, int index)
    {
        string? stringValue = argumentValue?.ToString();
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            throw new NullReferenceException($"{nameof(AttributeData.ConstructorArguments)} at {index} is null.");
        }
        return stringValue!;
    }

    #endregion
}