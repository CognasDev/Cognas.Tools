using Cognas.ApiTools.Shared;
using Microsoft.Extensions.Logging;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
public abstract class ModelIdServiceBusinessLogic : LoggerBusinessLogicBase, IModelIdServiceBusinessLogic
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public IModelIdService ModelIdService { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="ModelIdServiceBusinessLogic"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="modelIdService"></param>
    protected ModelIdServiceBusinessLogic(ILogger logger, IModelIdService modelIdService) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(modelIdService, nameof(modelIdService));
        ModelIdService = modelIdService;
    }

    #endregion
}