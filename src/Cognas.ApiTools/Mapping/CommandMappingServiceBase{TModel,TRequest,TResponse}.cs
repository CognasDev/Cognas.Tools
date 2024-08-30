using Cognas.Tools.Shared.Extensions;
using System.Collections.Frozen;

namespace Cognas.ApiTools.Mapping;

/// <summary>
/// <see href="https://docs.mappinggenerator.net/mappings/mapping-methods" />
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class CommandMappingServiceBase<TModel, TRequest, TResponse> : ICommandMappingService<TModel, TRequest, TResponse>
    where TModel : class
    where TRequest : notnull
    where TResponse : class
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandMappingServiceBase{TModel,TRequest,TResponse}"/>
    /// </summary>
    protected CommandMappingServiceBase()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public abstract TModel RequestToModel(TRequest request);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requests"></param>
    public IEnumerable<TModel> RequestsToModels(IEnumerable<TRequest> requests)
    {
        List<TModel> models = [];
        requests.FastForEach(request =>
        {
            TModel model = RequestToModel(request);
            models.Add(model);
        });
        return models.ToFrozenSet();
    }

    #endregion
}