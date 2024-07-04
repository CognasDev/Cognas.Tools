using System.Windows.Input;

namespace Cognas.MaulTools.Shared.Commands;

/// <summary>
/// 
/// </summary>
public abstract class DelegateCommandBase : ICommand
{
    #region Field Declarations

    private readonly SynchronizationContext? _synchronizationContext;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    protected DelegateCommandBase() => _synchronizationContext = SynchronizationContext.Current;

    #endregion

    #region Event Handler Declarations

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    protected abstract bool CanExecute(object? parameter);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    protected abstract void Execute(object? parameter);

    /// <summary>
    /// 
    /// </summary>
    public void RaiseCanExecuteChanged() => OnCanExecuteChanged();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    bool ICommand.CanExecute(object? parameter) => CanExecute(parameter);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    void ICommand.Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            Execute(parameter);
        }
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    private void OnCanExecuteChanged()
    {
        EventHandler? canExecuteChangedEventHandler = CanExecuteChanged;
        if (canExecuteChangedEventHandler is not null)
        {
            if (_synchronizationContext is not null && _synchronizationContext != SynchronizationContext.Current)
            {
                _synchronizationContext.Post(parameter => canExecuteChangedEventHandler.Invoke(this, EventArgs.Empty), null);
            }
            else
            {
                canExecuteChangedEventHandler.Invoke(this, EventArgs.Empty);
            }
        }
    }

    #endregion
}