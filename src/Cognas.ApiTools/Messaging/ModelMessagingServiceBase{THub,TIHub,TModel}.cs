using Microsoft.AspNetCore.SignalR;

namespace Cognas.ApiTools.Messaging;

/// <summary>
/// 
/// </summary>
public abstract class ModelMessagingServiceBase<THub, TIHub, TModel> : IModelMessagingService<TModel>
    where THub : Hub<TIHub>
    where TIHub : class, IModelHub<TModel>
    where TModel : class
{
    #region Field Declarations

    private readonly IHubContext<THub, TIHub> _hubContext;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ModelMessagingServiceBase{THub,TIHub,TModel}"/>
    /// </summary>
    /// <param name="hubContext"></param>
    protected ModelMessagingServiceBase(IHubContext<THub, TIHub> hubContext)
    {
        ArgumentNullException.Equals(hubContext, nameof(hubContext));
        _hubContext = hubContext;
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
        if (model != null)
        {
            await _hubContext.Clients.All.OnInsertModelAsync(model).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task OnUpdateModelAsync(TModel? model)
    {
        if (model != null)
        {
            await _hubContext.Clients.All.OnUpdateModelAsync(model).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task OnDeleteModelAsync(TModel model)
    {
        if (model != null)
        {
            await _hubContext.Clients.All.OnDeleteModelAsync(model).ConfigureAwait(false);
        }
    }

    #endregion
}