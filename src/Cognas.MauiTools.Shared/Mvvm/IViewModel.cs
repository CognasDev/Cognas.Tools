using System.ComponentModel;

namespace Cognas.MaulTools.Shared.Mvvm;

/// <summary>
/// 
/// </summary>
public interface IViewModel : IDisposable, INotifyPropertyChanged
{
    #region Propety Declarations

    /// <summary>
    /// 
    /// </summary>
    Guid ViewModelId { get; }

    #endregion
}