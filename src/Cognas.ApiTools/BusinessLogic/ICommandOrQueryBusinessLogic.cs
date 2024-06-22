using Cognas.ApiTools.Shared;

namespace Cognas.ApiTools.BusinessLogic;

/// <summary>
/// 
/// </summary>
public interface ICommandOrQueryBusinessLogic : ILoggerBusinessLogic
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    IModelIdService ModelIdService { get; }

    #endregion
}