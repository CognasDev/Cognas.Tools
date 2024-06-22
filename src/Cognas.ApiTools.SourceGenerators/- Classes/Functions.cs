using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cognas.ApiTools.SourceGenerators;

/// <summary>
/// 
/// </summary>
internal static class Functions
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelAncestors"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static string GetModelNamespace(IEnumerable<SyntaxNode> modelAncestors)
    {
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
    /// <param name="record"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static string GetIdPropertyNames(RecordDeclarationSyntax record)
    {
        List<string> idPropertyNames = [];
        IEnumerable<PropertyDeclarationSyntax?> properties = record.Members
                                                                   .Select(memberDeclarationSyntax => memberDeclarationSyntax as PropertyDeclarationSyntax)
                                                                   .Where(propertyDeclarationSyntax => propertyDeclarationSyntax != null);
        foreach (PropertyDeclarationSyntax? propertyDeclaration in properties)
        {
            AttributeSyntax? idAttribute = propertyDeclaration!.AttributeLists
                                                               .SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
                                                               .Where(attributeSyntax => attributeSyntax.Name.ToString() == "Id")
                                                               .SingleOrDefault();
            if (idAttribute != null)
            {
                idPropertyNames.Add(propertyDeclaration.Identifier.Text);
            }
        }
        if (idPropertyNames.Count == 1)
        {
            return idPropertyNames[0];
        }
        if (idPropertyNames.Count > 1)
        {
            throw new InvalidOperationException($"Multipled Id attributes found on model '{record.Identifier}'.");
        }
        throw new KeyNotFoundException($"Id attribute not found on model '{record.Identifier}'.");
    }

    #endregion
}