using Cognas.ApiTools.Shared.Extensions;

namespace Cognas.ApiTools.Mapping;

/// <summary>
/// <see href="https://docs.mappinggenerator.net/mappings/mapping-methods" />
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class MappingServiceBase<TModel, TRequest, TResponse> : ICommandMappingService<TModel, TRequest, TResponse>
    where TModel : class
    where TRequest : class
    where TResponse : class
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="MappingServiceBase{TModel,TRequest,TResponse}"/>
    /// </summary>
    protected MappingServiceBase()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="MapRequestToModelNotSupportedException"></exception>
    public virtual TModel RequestToModel(TRequest request) => throw new MapRequestToModelNotSupportedException(typeof(TRequest), typeof(TModel));

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
        return models;
    }

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