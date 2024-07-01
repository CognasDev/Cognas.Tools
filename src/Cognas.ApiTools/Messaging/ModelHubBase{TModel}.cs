using Microsoft.AspNetCore.SignalR;

namespace Cognas.ApiTools.Messaging;

/// <summary>
/// 
/// </summary>
public abstract class ModelHubBase<TModel> : Hub<IModelHub<TModel>>, IModelHub<TModel> where TModel : class
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ModelHubBase{TModel}"/>
    /// </summary>
    protected ModelHubBase()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task OnInsertModelAsync(TModel? model)
    {
        if (model is not null)
        {
            await Clients.All.OnInsertModelAsync(model).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task OnUpdateModelAsync(TModel? model)
    {
        if (model is not null)
        {
            await Clients.All.OnUpdateModelAsync(model).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task OnDeleteModelAsync(TModel model) => await Clients.All.OnDeleteModelAsync(model).ConfigureAwait(false);

    #endregion
}