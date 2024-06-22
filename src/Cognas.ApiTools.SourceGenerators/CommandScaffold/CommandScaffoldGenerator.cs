using Cognas.ApiTools.SourceGenerators.CommandScaffold.Names;
using Cognas.ApiTools.SourceGenerators.QueryScaffold;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Linq;

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
            static postInitializationContext  => postInitializationContext.AddSource
            (
                SourceFileNames.CommandScaffoldAttribute,
                TemplateCache.GetTemplate(TemplateNames.CommandScaffoldAttribute)
            )
        );

        IncrementalValueProvider<ImmutableArray<CommandScaffoldDetail>> modelsToGenerate = context.SyntaxProvider.ForAttributeWithMetadataName
        (
            AttributeNames.CommandScaffoldAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax,
            transform: static (generatorSyntaxContext, _) => GetDetails(generatorSyntaxContext)
        ).WithTrackingName(TrackingNames.FindModelsWithGenerateCommandBusinessLogic)
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
    private static CommandScaffoldDetail GetDetails(GeneratorAttributeSyntaxContext generatorSyntaxContext)
    {
        RecordDeclarationSyntax modelDeclaration = (RecordDeclarationSyntax)generatorSyntaxContext.TargetNode;
        AttributeData commandScaffoldAttribute = generatorSyntaxContext.Attributes[0];
        string requestType = commandScaffoldAttribute.ConstructorArguments[0].Value!.ToString();
        string responseType = commandScaffoldAttribute.ConstructorArguments[1].Value!.ToString();
        int apiVersion = (int)commandScaffoldAttribute.ConstructorArguments[2].Value!;
        bool useMessaging = (bool)commandScaffoldAttribute.ConstructorArguments[3].Value!;
        string modelNamespace = Functions.GetModelNamespace(modelDeclaration!.Ancestors());
        string modelName = modelDeclaration!.Identifier.Text;
        CommandScaffoldDetail detail = new(modelNamespace, modelName, requestType, responseType, apiVersion, useMessaging);
        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    private void GenerateSource(SourceProductionContext context, ImmutableArray<CommandScaffoldDetail> details)
    {
        string commandApiTemplate = TemplateCache.GetTemplate(TemplateNames.CommandApi);
        foreach (CommandScaffoldDetail detail in details.OrderBy(item => item.ModelName))
        {
            GenerateApi(context, commandApiTemplate, detail);
            GenerateBusinessLogic(context, detail);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    private static void GenerateApi(SourceProductionContext context, string template, CommandScaffoldDetail detail)
    {
        string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
        string commandApiSource = string.Format(template,
                                                detail.ModelNamespace,
                                                detail.ModelName,
                                                fullModelName,
                                                detail.RequestName,
                                                detail.ResponseName);
        string versionFilename = string.Format(SourceFileNames.CommandApi, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, commandApiSource);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="detail"></param>
    private static void GenerateBusinessLogic(SourceProductionContext context, CommandScaffoldDetail detail)
    {
        string template = detail.UseMessaging ?
                          TemplateCache.GetTemplate(TemplateNames.CommandBusinessLogicMessaging) :
                          TemplateCache.GetTemplate(TemplateNames.CommandBusinessLogicNoMessaging);

        string fullModelName = $"{detail.ModelNamespace}.{detail.ModelName}";
        string commandBusinesssLogicClass = string.Format(template,
                                                          detail.ModelNamespace,
                                                          detail.ModelName,
                                                          fullModelName);
        string versionFilename = string.Format(SourceFileNames.CommandBusinessLogic, detail.ApiVersion);
        string filename = $"{detail.ModelName}.{versionFilename}";
        context.AddSource(filename, commandBusinesssLogicClass);
    }

    #endregion
}