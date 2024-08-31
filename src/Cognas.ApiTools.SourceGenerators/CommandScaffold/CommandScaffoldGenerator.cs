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
public sealed class CommandScaffoldGenerator : IIncrementalGenerator
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
    public void Initialize(IncrementalGeneratorInitializationContext context)
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

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="generatorSyntaxContext"></param>
    /// <returns></returns>
    private static List<CommandScaffoldDetail> GetDetails(GeneratorAttributeSyntaxContext generatorSyntaxContext)
    {
        RecordDeclarationSyntax modelDeclaration = (RecordDeclarationSyntax)generatorSyntaxContext.TargetNode;
        string modelNamespace = modelDeclaration.GetNamespace();
        string modelName = modelDeclaration.GetName();
        string idPropertyName = modelDeclaration.GetIdPropertyName();

        List<string> propertyNames = modelDeclaration.Members.Select(memberDeclarationSyntax => memberDeclarationSyntax)
                                                             .OfType<PropertyDeclarationSyntax>()
                                                             .Select(propertyDeclarationSyntax => propertyDeclarationSyntax.Identifier.Text).ToList();

        propertyNames.Remove(idPropertyName);

        List<CommandScaffoldDetail> details = [];
        foreach (AttributeData commandScaffoldAttribute in generatorSyntaxContext.Attributes)
        {
            string requestType = commandScaffoldAttribute.GetConstructorArgumentValue<string>(0);
            string responseType = commandScaffoldAttribute.GetConstructorArgumentValue<string>(1);
            int apiVersion = commandScaffoldAttribute.GetConstructorArgumentValue<int>(2);
            bool useMessaging = commandScaffoldAttribute.GetConstructorArgumentValue<bool>(3);
            bool useDefaultMapper = commandScaffoldAttribute.GetConstructorArgumentValue<bool>(4);
            CommandScaffoldDetail detail = new(modelNamespace, modelName, requestType, responseType, apiVersion, useMessaging, useDefaultMapper, idPropertyName, propertyNames);
            details.Add(detail);
        }
        return details;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    private void GenerateSource(SourceProductionContext context, ImmutableArray<List<CommandScaffoldDetail>> details)
    {
        string commandApiTemplate = TemplateCache.GetTemplate(TemplateNames.CommandApi);
        IEnumerable<CommandScaffoldDetail> detailsCollection = from detailsList in details
                                                               from detail in detailsList.OrderBy(detail => detail.ModelName)
                                                               select detail;
        ReadOnlySpan<CommandScaffoldDetail> detailsSpan = [.. detailsCollection];
        StringBuilder commandEndpointInitiatorBuilder = new();
        GenerateInitiateCommandEndpoints.ClearApiVersions();
        foreach (CommandScaffoldDetail detail in detailsSpan)
        {
            string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
            GenerateApi(context, fullModelName, commandApiTemplate, detail);
            GenerateBusinessLogic(context, fullModelName, detail);
            GenerateInitiateCommandEndpoints.Generate(commandEndpointInitiatorBuilder, detail);
            if (detail.UseDefaultMapper)
            {
                GenerateCommandMappingService(context, fullModelName, detail);  
            }
        }
        GenerateCommandEndpointInitiator(context, commandEndpointInitiatorBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    private static void GenerateApi(SourceProductionContext context, string fullModelName, string template, CommandScaffoldDetail detail)
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
    /// <param name="detail"></param>
    private static void GenerateBusinessLogic(SourceProductionContext context, string fullModelName, CommandScaffoldDetail detail)
    {
        string template = detail.UseMessaging ?
                          TemplateCache.GetTemplate(TemplateNames.CommandBusinessLogicMessaging) :
                          TemplateCache.GetTemplate(TemplateNames.CommandBusinessLogicNoMessaging);

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
    /// <param name="fullModelName"></param>
    /// <param name="detail"></param>
    private static void GenerateCommandMappingService(SourceProductionContext context, string fullModelName, CommandScaffoldDetail detail)
    {
        string template = TemplateCache.GetTemplate(TemplateNames.CommandMappingService);
        string propertyMaps = BuildPropertyMaps(detail);
        string commandMappingServiceSource = string.Format(template,
                                                           fullModelName,
                                                           detail.RequestName,
                                                           detail.ModelNamespace,
                                                           detail.ModelName,
                                                           propertyMaps);
        string filename = $"{detail.ModelName}.{SourceFileNames.CommandMappingService}";
        context.AddSource(filename, commandMappingServiceSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="detail"></param>
    /// <returns></returns>
    private static string BuildPropertyMaps(CommandScaffoldDetail detail)
    {
        StringBuilder propertyMapsBuilder = new();
        propertyMapsBuilder.Append("\t\t\t");
        propertyMapsBuilder.Append(detail.IdPropertyName);
        propertyMapsBuilder.Append(" = request.");
        propertyMapsBuilder.Append(detail.IdPropertyName);
        propertyMapsBuilder.AppendLine(" ?? NotInsertedId,");

        foreach (string propertyName in detail.PropertyNames)
        {
            propertyMapsBuilder.Append("\t\t\t");
            propertyMapsBuilder.Append(propertyName);
            propertyMapsBuilder.Append(" = request.");
            propertyMapsBuilder.Append(propertyName);
            propertyMapsBuilder.AppendLine(",");
        }

        string propertyMaps = propertyMapsBuilder.ToString();
        return propertyMaps;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="commandEndpointInitiatorBuilder"></param>
    private static void GenerateCommandEndpointInitiator(SourceProductionContext context, StringBuilder commandEndpointInitiatorBuilder)
    {
        string commandEndpointInitiatorTemplate = TemplateCache.GetTemplate(TemplateNames.CommandEndpointInitiator);
        string commandEndpointInitiatorSource = string.Format(commandEndpointInitiatorTemplate, commandEndpointInitiatorBuilder.ToString());
        context.AddSource(SourceFileNames.CommandEndpointInitiator, commandEndpointInitiatorSource);
    }

    #endregion
}