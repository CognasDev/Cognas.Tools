using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

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
    /// <param name="attributeData"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static string GetConstructorArgumentValue(this AttributeData attributeData, int index)
    {
        object? argumentValue = attributeData.ConstructorArguments[index].Value;
        return argumentValue is null ? throw new NullReferenceException($"{nameof(AttributeData.ConstructorArguments)} at {index} is null.") : argumentValue.ToString();
    }

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
        object? argumentValue = attributeData.ConstructorArguments[index].Value;
        return argumentValue is null ? throw new NullReferenceException($"{nameof(AttributeData.ConstructorArguments)} at {index} is null.") : (TValue)argumentValue;
    }

    #endregion
}