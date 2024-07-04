using Cognas.MauiTools.Shared;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cognas.MaulTools.Shared.Mvvm;

/// <summary>
/// 
/// </summary>
public abstract class NotifyPropertyChangedBase : DisposableBase, INotifyPropertyChanged
{
    #region Field Declarations

    private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> _propertyChangedEventArgsCache = new();

    #endregion

    #region Event Declarations

    /// <summary>
    /// 
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    protected NotifyPropertyChangedBase()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="field"></param>
    /// <param name="newValue"></param>
    /// <param name="propertyName"></param>
    public bool SetProperty<TProperty>(ref TProperty field, TProperty newValue, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));
        if (Equals(field, newValue))
        {
            return false;
        }
        field = newValue;
        RaiseCanExecuteChanged(propertyName);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected void RaiseCanExecuteChanged(string propertyName)
    {
        PropertyChangedEventArgs eventArgs = _propertyChangedEventArgsCache.GetOrAdd(propertyName, key => new PropertyChangedEventArgs(key));
        PropertyChanged?.Invoke(this, eventArgs);
    }

    #endregion
}