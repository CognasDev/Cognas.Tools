using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Cognas.ApiTools.SourceGenerators;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TDetail"></typeparam>
public abstract class GeneratorBase<TDetail> : IIncrementalGenerator
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    protected ApiVersionRepsitory ApiVersionRepsitory { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    protected DefaultMapperGenerationState DefaultMapperGenerationState { get; } = new();

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="GeneratorBase{TModel}"/>
    /// </summary>
    protected GeneratorBase()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);

    #endregion

    #region Protected Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="details"></param>
    protected abstract void GenerateSource(SourceProductionContext context, ImmutableArray<List<TDetail>> details);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    protected abstract void GenerateApi(SourceProductionContext context, string fullModelName, string template, TDetail detail);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fullModelName"></param>
    /// <param name="template"></param>
    /// <param name="detail"></param>
    protected abstract void GenerateBusinessLogic(SourceProductionContext context, string fullModelName, string template, TDetail detail);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="endpointInitiatorBuilder"></param>
    protected abstract void GenerateEndpointInitiator(SourceProductionContext context, StringBuilder endpointInitiatorBuilder);

    #endregion
}