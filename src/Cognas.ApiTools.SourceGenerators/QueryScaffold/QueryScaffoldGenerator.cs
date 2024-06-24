using Cognas.ApiTools.SourceGenerators.QueryScaffold.Names;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Cognas.ApiTools.SourceGenerators.QueryScaffold;

/// <summary>
/// 
/// </summary>
[Generator]
public sealed class QueryScaffoldGenerator : IIncrementalGenerator
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryScaffoldGenerator"/>
    /// </summary>
    public QueryScaffoldGenerator()
    {
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //        {
        //            System.Diagnostics.Debugger.Launch();
        //        }
        //#endif
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput
        (
            static postInitializationContext => postInitializationContext.AddSource
            (
                SourceFileNames.QueryScaffoldAttribute,
                TemplateCache.GetTemplate(TemplateNames.QueryScaffoldAttribute)
            )
        );

        IncrementalValueProvider<ImmutableArray<QueryScaffoldDetail>> queryScaffoldDetails = context.SyntaxProvider.ForAttributeWithMetadataName
        (
            AttributeNames.QueryScaffoldAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax,
            transform: static (generatorSyntaxContext, _) => GetDetails(generatorSyntaxContext)
        )
        .WithTrackingName(TrackingNames.FindQueryScaffoldAttributes)
        .Collect()
        .WithTrackingName(TrackingNames.CollectQueryScaffoldDetails);

        context.RegisterSourceOutput(queryScaffoldDetails, GenerateSource);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="generatorSyntaxContext"></param>
    /// <returns></returns>
    private static QueryScaffoldDetail GetDetails(GeneratorAttributeSyntaxContext generatorSyntaxContext)
    {
        RecordDeclarationSyntax modelDeclaration = (RecordDeclarationSyntax)generatorSyntaxContext.TargetNode;
        AttributeData queryScaffoldAttribute = generatorSyntaxContext.Attributes[0];
        string responseType = queryScaffoldAttribute.GetConstructorArgumentValue<string>(0);
        int apiVersion = queryScaffoldAttribute.GetConstructorArgumentValue<int>(1);
        string modelNamespace = modelDeclaration.GetNamespace();
        string modelName = modelDeclaration.GetName();
        QueryScaffoldDetail detail = new(modelNamespace, modelName, responseType, apiVersion);
        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    private void GenerateSource(SourceProductionContext context, ImmutableArray<QueryScaffoldDetail> details)
    {
        string queryApiTemplate = TemplateCache.GetTemplate(TemplateNames.QueryApi);
        string queryBusinessLogicTemplate = TemplateCache.GetTemplate(TemplateNames.QueryBusinessLogic);
        ReadOnlySpan<QueryScaffoldDetail> detailsSpan = [.. details.OrderBy(detail => detail.ModelName)];
        foreach (QueryScaffoldDetail detail in detailsSpan)
        {
            string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
            GenerateApi(context, fullModelName, queryApiTemplate, detail);
            GenerateBusinessLogic(context, fullModelName, queryBusinessLogicTemplate, detail);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    private static void GenerateApi(SourceProductionContext context, string fullModelName, string template, QueryScaffoldDetail detail)
    {
        string queryApiSource = string.Format(template,
                                              fullModelName,
                                              detail.ResponseName,
                                              detail.ModelNamespace,
                                              detail.ApiVersion,
                                              detail.ModelName);
        string versionFilename = string.Format(SourceFileNames.QueryApi, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, queryApiSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    private static void GenerateBusinessLogic(SourceProductionContext context, string fullModelName, string template, QueryScaffoldDetail detail)
    {
        string queryBusinesssLogicSource = string.Format(template,
                                                         fullModelName,
                                                         detail.ModelNamespace,
                                                         detail.ApiVersion,
                                                         detail.ModelName);
        string versionFilename = string.Format(SourceFileNames.QueryBusinessLogic, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, queryBusinesssLogicSource);
    }

    #endregion
}