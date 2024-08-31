using Cognas.ApiTools.SourceGenerators.ModelIdService.Names;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators.ModelIdService;

/// <summary>
/// 
/// </summary>
[Generator]
public sealed class ModelIdServiceGenerator : IIncrementalGenerator
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ModelIdServiceGenerator"/>
    /// </summary>
    public ModelIdServiceGenerator()
    {
        //#if DEBUG
        //    if (!System.Diagnostics.Debugger.IsAttached)
        //    {
        //        System.Diagnostics.Debugger.Launch();
        //    }
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
            static postInitializationContext =>
            {
                postInitializationContext.AddSource
                (
                    GeneratedFileNames.IdAttribute,
                    TemplateCache.GetTemplate(TemplateNames.IdAttribute)
                );
                postInitializationContext.AddSource
                (
                    GeneratedFileNames.IncludeInModelIdServiceAttribute,
                    TemplateCache.GetTemplate(TemplateNames.IncludeInModelIdServiceAttribute)
                );
            }
        );

        IncrementalValueProvider<ImmutableArray<ModelIdServiceEntryDetail>> modelIdServiceEntryDetails = context.SyntaxProvider.ForAttributeWithMetadataName
        (
            AttributeNames.IncludeInModelIdServiceAttribute,
            predicate: static (syntaxNode, _) => syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax,
            transform: static (generatorSyntaxContext, _) => GetDetails((RecordDeclarationSyntax)generatorSyntaxContext.TargetNode)
        )
        .WithTrackingName(TrackingNames.FindIncludeInModelIdServiceAttributes)
        .Collect()
        .WithTrackingName(TrackingNames.CollectModelIdServiceEntryDetails);

        context.RegisterSourceOutput(modelIdServiceEntryDetails, GenerateSource);
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelDeclaration"></param>
    /// <returns></returns>
    private static ModelIdServiceEntryDetail GetDetails(RecordDeclarationSyntax modelDeclaration)
    {
        string modelNamespace = modelDeclaration.GetNamespace();
        string modelName = modelDeclaration.GetName();
        string idPropertyName = modelDeclaration.GetIdPropertyName();
        ModelIdServiceEntryDetail detail = new(modelNamespace, modelName, idPropertyName);
        return detail;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    private void GenerateSource(SourceProductionContext context, ImmutableArray<ModelIdServiceEntryDetail> details)
    {
        StringBuilder getIdsBuilder = new();
        StringBuilder setIdsBuilder = new();
        StringBuilder idExpressionBuilder = new();
        StringBuilder getModelIdNameBuilder = new();

        string template = TemplateCache.GetTemplate(TemplateNames.ModelIdService);
        ReadOnlySpan<ModelIdServiceEntryDetail> detailsSpan = [.. details.OrderBy(detail => detail.ModelName)];
        foreach (ModelIdServiceEntryDetail detail in detailsSpan)
        {
            GenerateGetId.Generate(getIdsBuilder, detail);
            GenerateSetIdValue.Generate(setIdsBuilder, detail);
            GenerateIdParameter.Generate(idExpressionBuilder, detail);
            GenerateGetModelIdName.Generate(getModelIdNameBuilder, detail);
        }

        string getIds = getIdsBuilder.ToString();
        string setIds = setIdsBuilder.ToString();
        string idExpression = idExpressionBuilder.ToString();
        string getModelIdName = getModelIdNameBuilder.ToString();

        string modelIdServiceSource = string.Format(template, getIds, setIds, idExpression, getModelIdName);
        context.AddSource(GeneratedFileNames.ModelIdService, modelIdServiceSource);
    }

    #endregion
}