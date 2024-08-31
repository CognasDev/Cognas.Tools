namespace Cognas.ApiTools.Data.Exceptions;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public sealed class UpdateModelException<TModel> : Exception
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public TModel Model { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="UpdateModelException{TModel}"/>
    /// </summary>
    /// <param name="model"></param>
    public UpdateModelException(TModel model) : base($"Update failed for model {typeof(TModel).FullName}.")
    {
        Model = model;
    }

    #endregion
}