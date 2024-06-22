using Cognas.ApiTools.SourceGenerators.QueryScaffold.Names;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System;

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

        IncrementalValueProvider<ImmutableArray<QueryScaffoldDetail>> modelsToGenerate = context.SyntaxProvider.ForAttributeWithMetadataName
        (
            AttributeNames.QueryScaffoldAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax,
            transform: static (generatorSyntaxContext, _) => GetDetails(generatorSyntaxContext)
        ).WithTrackingName(TrackingNames.FindModelsWithGenerateQueryBusinessLogic)
        .Collect().WithTrackingName(TrackingNames.CollectModelsIntoArray);

        context.RegisterSourceOutput(modelsToGenerate, GenerateSource);
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
        string responseType = queryScaffoldAttribute.ConstructorArguments[0].Value!.ToString();
        int apiVersion = (int)queryScaffoldAttribute.ConstructorArguments[1].Value!;
        string modelNamespace = Functions.GetModelNamespace(modelDeclaration!.Ancestors());
        string modelName = modelDeclaration!.Identifier.Text;
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
        foreach (QueryScaffoldDetail detail in details.OrderBy(detail => detail.ModelName))
        {
            GenerateApi(context, queryApiTemplate, detail);
            GenerateBusinessLogic(context, queryBusinessLogicTemplate, detail);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    private static void GenerateApi(SourceProductionContext context, string template, QueryScaffoldDetail detail)
    {
        string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
        string queryApiSource = string.Format(template,
                                              detail.ModelNamespace,
                                              detail.ModelName,
                                              fullModelName,
                                              detail.ResponseName);
        string versionFilename = string.Format(SourceFileNames.QueryApi, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, queryApiSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    private static void GenerateBusinessLogic(SourceProductionContext context, string template, QueryScaffoldDetail detail)
    {
        string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
        string queryBusinesssLogicSource = string.Format(template,
                                                         detail.ModelNamespace,
                                                         detail.ModelName,
                                                         fullModelName);
        string versionFilename = string.Format(SourceFileNames.QueryBusinessLogic, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, queryBusinesssLogicSource);
    }

    #endregion
}