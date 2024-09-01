using Cognas.ApiTools.SourceGenerators.CommandScaffold;
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
public sealed class QueryScaffoldGenerator : IIncrementalGenerator
{
    #region Field Declarations

    private readonly ApiVersionRepsitory _apiVersionRepsitory = new();
    private readonly DefaultMapperGenerationState _defaultMapperGenerationState = new();

    #endregion

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
            string responseType = queryScaffoldAttribute.GetConstructorArgumentValue<string>(0);
            int apiVersion = queryScaffoldAttribute.GetConstructorArgumentValue<int>(1);
            bool useDefaultMapper = queryScaffoldAttribute.GetConstructorArgumentValue<bool>(2);
            QueryScaffoldDetail detail = new(modelNamespace, modelName, responseType, apiVersion, useDefaultMapper, idPropertyName, propertyNames);
            details.Add(detail);
        }
        return details;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    private void GenerateSource(SourceProductionContext context, ImmutableArray<List<QueryScaffoldDetail>> details)
    {
        string queryApiTemplate = TemplateCache.GetTemplate(TemplateNames.QueryApi);
        string queryBusinessLogicTemplate = TemplateCache.GetTemplate(TemplateNames.QueryBusinessLogic);
        IEnumerable<QueryScaffoldDetail> detailsCollection = from detailsList in details
                                                             from detail in detailsList.OrderBy(detail => detail.ModelName)
                                                             select detail;
        ReadOnlySpan<QueryScaffoldDetail> detailsSpan = [.. detailsCollection];
        StringBuilder queryEndpointInitiatorBuilder = new();
        _apiVersionRepsitory.Clear();
        foreach (QueryScaffoldDetail detail in detailsSpan)
        {
            string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
            GenerateApi(context, fullModelName, queryApiTemplate, detail);
            GenerateBusinessLogic(context, fullModelName, queryBusinessLogicTemplate, detail);
            queryEndpointInitiatorBuilder.GenerateInitiateQueryEndpoints(detail, _apiVersionRepsitory);
            if (detail.UseDefaultMapper && !_defaultMapperGenerationState.IsGenerated(fullModelName))
            {
                QueryMappingServiceGenerator.Generate(context, fullModelName, detail);
                _defaultMapperGenerationState.SetGenerated(fullModelName);
            }
        }
        GenerateQueryEndpointInitiator(context, queryEndpointInitiatorBuilder);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="queryEndpointInitiatorBuilder"></param>
    private static void GenerateQueryEndpointInitiator(SourceProductionContext context, StringBuilder queryEndpointInitiatorBuilder)
    {
        string queryEndpointInitiatorTemplate = TemplateCache.GetTemplate(TemplateNames.QueryEndpointInitiator);
        string queryEndpointInitiatorSource = string.Format(queryEndpointInitiatorTemplate, queryEndpointInitiatorBuilder.ToString());
        context.AddSource(SourceFileNames.QueryEndpointInitiator, queryEndpointInitiatorSource);
    }

    #endregion
}