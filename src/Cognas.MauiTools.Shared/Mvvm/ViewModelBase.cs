﻿namespace Cognas.MaulTools.Shared.Mvvm;

/// <summary>
/// 
/// </summary>
public abstract class ViewModelBase : NotifyPropertyChangedBase, IViewModel
{
    #region Property Declarations

    /// <summary>
    /// 
    /// </summary>
    public Guid ViewModelId { get; } = Guid.NewGuid();

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    protected ViewModelBase()
    {
    }

    #endregion
}