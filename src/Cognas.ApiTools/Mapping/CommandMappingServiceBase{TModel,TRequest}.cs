using Cognas.Tools.Shared.Extensions;
using System.Collections.Frozen;

namespace Cognas.ApiTools.Mapping;

/// <summary>
/// <see href="https://docs.mappinggenerator.net/mappings/mapping-methods" />
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
public abstract class CommandMappingServiceBase<TModel, TRequest> : ICommandMappingService<TModel, TRequest>
    where TModel : class
    where TRequest : notnull
{
    #region Field Declarations

    /// <summary>
    /// 
    /// </summary>
    protected const int NotInsertedId = -1;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="CommandMappingServiceBase{TModel,TRequest}"/>
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