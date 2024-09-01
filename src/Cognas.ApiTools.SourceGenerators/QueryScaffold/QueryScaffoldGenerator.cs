using Cognas.ApiTools.SourceGenerators.QueryScaffold.Names;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators.QueryScaffold;

/// <summary>
/// 
/// </summary>
[Generator]
public sealed class QueryScaffoldGenerator : GeneratorBase<QueryScaffoldDetail>
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
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput
        (
            static postInitializationContext => postInitializationContext.AddSource
            (
                SourceFileNames.QueryScaffoldAttribute,
                TemplateCache.GetTemplate(TemplateNames.QueryScaffoldAttribute)
            )
        );

        IncrementalValueProvider<ImmutableArray<List<QueryScaffoldDetail>>> queryScaffoldDetails = context.SyntaxProvider.ForAttributeWithMetadataName
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

    #region Protected Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    protected override void GenerateSource(SourceProductionContext context, ImmutableArray<List<QueryScaffoldDetail>> details)
    {
        string queryApiTemplate = TemplateCache.GetTemplate(TemplateNames.QueryApi);
        string queryBusinessLogicTemplate = TemplateCache.GetTemplate(TemplateNames.QueryBusinessLogic);
        IEnumerable<QueryScaffoldDetail> detailsCollection = from detailsList in details
                                                             from detail in detailsList.OrderBy(detail => detail.ModelName)
                                                             select detail;
        ReadOnlySpan<QueryScaffoldDetail> detailsSpan = [.. detailsCollection];
        StringBuilder queryEndpointInitiatorBuilder = new();
        ApiVersionRepsitory.Clear();
        foreach (QueryScaffoldDetail detail in detailsSpan)
        {
            string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
            GenerateApi(context, fullModelName, queryApiTemplate, detail);
            GenerateBusinessLogic(context, fullModelName, queryBusinessLogicTemplate, detail);
            queryEndpointInitiatorBuilder.GenerateInitiateQueryEndpoints(detail, ApiVersionRepsitory);
            if (detail.UseDefaultMapper && !DefaultMapperGenerationState.IsGenerated(fullModelName))
            {
                QueryMappingServiceGenerator.Generate(context, fullModelName, detail);
                DefaultMapperGenerationState.SetGenerated(fullModelName);
            }
        }
        GenerateEndpointInitiator(context, queryEndpointInitiatorBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    protected override void GenerateApi(SourceProductionContext context, string fullModelName, string template, QueryScaffoldDetail detail)
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
    protected override void GenerateBusinessLogic(SourceProductionContext context, string fullModelName, string template, QueryScaffoldDetail detail)
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="endpointInitiatorBuilder"></param>
    protected override void GenerateEndpointInitiator(SourceProductionContext context, StringBuilder endpointInitiatorBuilder)
    {
        string queryEndpointInitiatorTemplate = TemplateCache.GetTemplate(TemplateNames.QueryEndpointInitiator);
        string endpoints = endpointInitiatorBuilder.ToString();
        string queryEndpointInitiatorSource = string.Format(queryEndpointInitiatorTemplate, endpoints);
        context.AddSource(SourceFileNames.QueryEndpointInitiator, queryEndpointInitiatorSource);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="generatorSyntaxContext"></param>
    /// <returns></returns>
    private static List<QueryScaffoldDetail> GetDetails(GeneratorAttributeSyntaxContext generatorSyntaxContext)
    {
        RecordDeclarationSyntax modelDeclaration = generatorSyntaxContext.GetModelDeclaration();
        string modelNamespace = modelDeclaration.GetNamespace();
        string modelName = modelDeclaration.GetName();
        string idPropertyName = modelDeclaration.GetIdPropertyName();

        IList<string> propertyNames = modelDeclaration.GetModelProperties();
        propertyNames.Remove(idPropertyName);

        List<QueryScaffoldDetail> details = [];
        foreach (AttributeData queryScaffoldAttribute in generatorSyntaxContext.Attributes)
        {
            QueryScaffoldDetail detail = CreateDetail(modelNamespace, modelName, idPropertyName, propertyNames, queryScaffoldAttribute);
            details.Add(detail);
        }
        return details;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelNamespace"></param>
    /// <param name="modelName"></param>
    /// <param name="idPropertyName"></param>
    /// <param name="propertyNames"></param>
    /// <param name="queryScaffoldAttribute"></param>
    /// <returns></returns>
    private static QueryScaffoldDetail CreateDetail(string modelNamespace, string modelName, string idPropertyName, IList<string> propertyNames, AttributeData queryScaffoldAttribute)
    {
        string responseType = queryScaffoldAttribute.GetConstructorArgumentValue<string>(0);
        int apiVersion = queryScaffoldAttribute.GetConstructorArgumentValue<int>(1);
        bool useDefaultMapper = queryScaffoldAttribute.GetConstructorArgumentValue<bool>(2);
        QueryScaffoldDetail detail = new(modelNamespace, modelName, responseType, apiVersion, useDefaultMapper, idPropertyName, propertyNames);
        return detail;
    }

    #endregion
}