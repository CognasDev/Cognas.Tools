namespace Cognas.MaulTools.Shared.Commands;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public sealed class DelegateCommand<TParameter> : DelegateCommandBase where TParameter : notnull
{
    #region Field Declarations

    private readonly Action<TParameter?> _executeMethod;
    private readonly Func<TParameter?, bool> _canExecuteMethod;

    #endregion

    #region Construction and Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="executeMethod"></param>
    public DelegateCommand(Action<TParameter?> executeMethod) : this(executeMethod, parameter => true)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="executeMethod"></param>
    /// <param name="canExecuteMethod"></param>
    public DelegateCommand(Action<TParameter?> executeMethod, Func<TParameter?, bool> canExecuteMethod) : base()
    {
        _executeMethod = executeMethod;
        _canExecuteMethod = canExecuteMethod;
    }

    #endregion

    #region Overridden Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    protected override bool CanExecute(object? parameter)
    {
        TParameter? stronglyTypedParameter = (TParameter?)parameter;
        return CanExecute(stronglyTypedParameter);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    protected override void Execute(object? parameter)
    {
        TParameter? stronglyTypedParameter = (TParameter?)parameter;
        Execute(stronglyTypedParameter);
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(TParameter? parameter) => _canExecuteMethod(parameter);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(TParameter? parameter)
    {
        if (_canExecuteMethod(parameter))
        {
            _executeMethod(parameter);
        }
    }

    #endregion
}