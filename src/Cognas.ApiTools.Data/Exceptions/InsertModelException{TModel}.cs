namespace Cognas.ApiTools.Data.Exceptions;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public sealed class InsertModelException<TModel> : Exception
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public TModel Model { get; }

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="InsertModelException{TModel}"/>
    /// </summary>
    /// <param name="model"></param>
    public InsertModelException(TModel model) : base($"Insert failed for model {typeof(TModel).FullName}.")
    {
        Model = model;
    }

    #endregion
}