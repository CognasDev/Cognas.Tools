using Cognas.ApiTools.SourceGenerators.CommandScaffold.Names;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators.CommandScaffold;

/// <summary>
/// 
/// </summary>
[Generator]
public sealed class CommandScaffoldGenerator : GeneratorBase<CommandScaffoldDetail>
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandScaffoldGenerator"/>
    /// </summary>
    public CommandScaffoldGenerator()
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
                SourceFileNames.CommandScaffoldAttribute,
                TemplateCache.GetTemplate(TemplateNames.CommandScaffoldAttribute)
            )
        );

        IncrementalValueProvider<ImmutableArray<List<CommandScaffoldDetail>>> commandScaffoldDetails = context.SyntaxProvider.ForAttributeWithMetadataName
        (
            AttributeNames.CommandScaffoldAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax,
            transform: static (generatorSyntaxContext, _) => GetDetails(generatorSyntaxContext)
        )
        .WithTrackingName(TrackingNames.FindCommandScaffoldAttributes)
        .Collect()
        .WithTrackingName(TrackingNames.CollectCommandScaffoldDetails);

        context.RegisterSourceOutput(commandScaffoldDetails, GenerateSource);
    }

    #endregion

    #region Protected Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    protected override void GenerateSource(SourceProductionContext context, ImmutableArray<List<CommandScaffoldDetail>> details)
    {
        string commandApiTemplate = TemplateCache.GetTemplate(TemplateNames.CommandApi);
        IEnumerable<CommandScaffoldDetail> detailsCollection = from detailsList in details
                                                               from detail in detailsList.OrderBy(detail => detail.ModelName)
                                                               select detail;
        ReadOnlySpan<CommandScaffoldDetail> detailsSpan = [.. detailsCollection];
        StringBuilder commandEndpointInitiatorBuilder = new();
        ApiVersionRepsitory.Clear();
        foreach (CommandScaffoldDetail detail in detailsSpan)
        {
            string businessLogicTemplate = detail.UseMessaging ?
                                                  TemplateCache.GetTemplate(TemplateNames.CommandBusinessLogicMessaging) :
                                                  TemplateCache.GetTemplate(TemplateNames.CommandBusinessLogicNoMessaging);

            string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
            GenerateApi(context, fullModelName, commandApiTemplate, detail);
            GenerateBusinessLogic(context, fullModelName, businessLogicTemplate, detail);
            commandEndpointInitiatorBuilder.GenerateInitiateCommandEndpoints(detail, ApiVersionRepsitory);
            if (detail.UseDefaultMapper && !DefaultMapperGenerationState.IsGenerated(detail.RequestName, fullModelName))
            {
                CommandMappingServiceGenerator.Generate(context, fullModelName, detail);
                DefaultMapperGenerationState.SetGenerated(detail.RequestName, fullModelName);
            }
        }
        GenerateEndpointInitiator(context, commandEndpointInitiatorBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    protected override void GenerateApi(SourceProductionContext context, string fullModelName, string template, CommandScaffoldDetail detail)
    {
        string commandApiSource = string.Format(template,
                                                fullModelName,
                                                detail.RequestName,
                                                detail.ResponseName,
                                                detail.ModelNamespace,
                                                detail.ApiVersion,
                                                detail.ModelName);
        string versionFilename = string.Format(SourceFileNames.CommandApi, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, commandApiSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    protected override void GenerateBusinessLogic(SourceProductionContext context, string fullModelName, string template, CommandScaffoldDetail detail)
    {
        string commandBusinesssLogicSource = string.Format(template,
                                                           fullModelName,
                                                           detail.ModelNamespace,
                                                           detail.ApiVersion,
                                                           detail.ModelName);
        string versionFilename = string.Format(SourceFileNames.CommandBusinessLogic, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, commandBusinesssLogicSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="endpointInitiatorBuilder"></param>
    protected override void GenerateEndpointInitiator(SourceProductionContext context, StringBuilder endpointInitiatorBuilder)
    {
        string commandEndpointInitiatorTemplate = TemplateCache.GetTemplate(TemplateNames.CommandEndpointInitiator);
        string endpoints = endpointInitiatorBuilder.ToString();
        string commandEndpointInitiatorSource = string.Format(commandEndpointInitiatorTemplate, endpoints);
        context.AddSource(SourceFileNames.CommandEndpointInitiator, commandEndpointInitiatorSource);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="generatorSyntaxContext"></param>
    /// <returns></returns>
    private static List<CommandScaffoldDetail> GetDetails(GeneratorAttributeSyntaxContext generatorSyntaxContext)
    {
        RecordDeclarationSyntax modelDeclaration = generatorSyntaxContext.GetModelDeclaration();
        string modelNamespace = modelDeclaration.GetNamespace();
        string modelName = modelDeclaration.GetName();
        string idPropertyName = modelDeclaration.GetIdPropertyName();

        IList<string> propertyNames = modelDeclaration.GetModelProperties();
        propertyNames.Remove(idPropertyName);

        List<CommandScaffoldDetail> details = [];
        foreach (AttributeData commandScaffoldAttribute in generatorSyntaxContext.Attributes)
        {
            CommandScaffoldDetail detail = CreateDetail(modelNamespace, modelName, idPropertyName, propertyNames, commandScaffoldAttribute);
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
    /// <param name="commandScaffoldAttribute"></param>
    /// <returns></returns>
    private static CommandScaffoldDetail CreateDetail(string modelNamespace, string modelName, string idPropertyName, IList<string> propertyNames, AttributeData commandScaffoldAttribute)
    {
        string requestType = commandScaffoldAttribute.GetConstructorArgumentValue<string>(0);
        string responseType = commandScaffoldAttribute.GetConstructorArgumentValue<string>(1);
        int apiVersion = commandScaffoldAttribute.GetConstructorArgumentValue<int>(2);
        bool useDefaultMapper = commandScaffoldAttribute.GetConstructorArgumentValue<bool>(3);
        bool useMessaging = commandScaffoldAttribute.GetConstructorArgumentValue<bool>(4);
        CommandScaffoldDetail detail = new(modelNamespace, modelName, requestType, responseType, apiVersion, useMessaging, useDefaultMapper, idPropertyName, propertyNames);
        return detail;
    }

    #endregion
}