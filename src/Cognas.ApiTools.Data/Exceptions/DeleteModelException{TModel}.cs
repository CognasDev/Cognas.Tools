namespace Cognas.ApiTools.Data.Exceptions;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TModel"></typeparam>
public sealed class DeleteModelException<TModel> : Exception
{
    #region Constructor / Finaliser Declarations

    /// <summary>
    /// Default constructor for <see cref="DeleteModelException{TModel}"/>
    /// </summary>
    public DeleteModelException() : base($"Delete failed for model {typeof(TModel).FullName}.")
    {
    }

    #endregion
}