namespace Cognas.MauiTools.Shared;

/// <summary>
/// 
/// </summary>
public abstract class DisposableBase : IDisposable
{
    #region Field Declarations

    private bool _isDisposed;

    #endregion

    #region Constructor / Finaliser Declarations

    /// <summary>
    /// 
    /// </summary>
    protected DisposableBase()
    {
    }

    #endregion

    #region Public Method Declarations

    /// <summary>
    ///     
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DisposeManagedResources()
    {
        return;
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void DisposeUnmanagedResources()
    {
        return;
    }

    #endregion

    #region Private Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
        {
            DisposeManagedResources();
            _isDisposed = true;
        }
        DisposeUnmanagedResources();
    }

    #endregion
}