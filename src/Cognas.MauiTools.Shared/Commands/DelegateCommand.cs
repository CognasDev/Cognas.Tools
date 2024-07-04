namespace Cognas.MaulTools.Shared.Commands;

/// <summary>
/// 
/// </summary>
public sealed class DelegateCommand : DelegateCommandBase
{
    #region Field Declarations

    private readonly Action _executeMethod;
    private readonly Func<bool> _canExecuteMethod;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="executeMethod"></param>
    public DelegateCommand(Action executeMethod) : this(executeMethod, () => true)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="executeMethod"></param>
    /// <param name="canExecuteMethod"></param>
    public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : base()
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
    protected override bool CanExecute(object? parameter) => CanExecute();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    protected override void Execute(object? parameter) => Execute();

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool CanExecute() => _canExecuteMethod();

    /// <summary>
    /// 
    /// </summary>
    public void Execute()
    {
        if (_canExecuteMethod())
        {
            _executeMethod();
        }
    }

    #endregion
}