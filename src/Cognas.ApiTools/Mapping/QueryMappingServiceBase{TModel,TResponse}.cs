using Cognas.Tools.Shared.Extensions;

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
        List<TResponse> dtos = [];
        models.FastForEach(model =>
        {
            TResponse dto = ModelToResponse(model);
            dtos.Add(dto);
        });
        return dtos;
    }

    #endregion
}