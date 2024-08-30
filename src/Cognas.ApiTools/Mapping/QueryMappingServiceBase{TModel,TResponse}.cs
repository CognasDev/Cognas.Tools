using Cognas.Tools.Shared.Extensions;
using System.Collections.Frozen;

namespace Cognas.ApiTools.Mapping;

/// <summary>
/// <see href="https://docs.mappinggenerator.net/mappings/mapping-methods" />
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class QueryMappingServiceBase<TModel, TResponse> : IQueryMappingService<TModel, TResponse>
    where TModel : class
    where TResponse : class
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="QueryMappingServiceBase{TModel,TResponse}"/>
    /// </summary>
    protected QueryMappingServiceBase()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public abstract TResponse ModelToResponse(TModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public IEnumerable<TResponse> ModelsToResponses(IEnumerable<TModel> models)
    {
        List<TResponse> responses = [];
        models.FastForEach(model =>
        {
            TResponse response = ModelToResponse(model);
            responses.Add(response);
        });
        return responses.ToFrozenSet();
    }

    #endregion
}