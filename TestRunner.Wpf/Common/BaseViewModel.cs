using CommunityToolkit.Mvvm.ComponentModel;
using System.Reactive.Disposables;

namespace VRT.Competitions.TestRunner.Wpf.Common;
public abstract class BaseViewModel : ObservableObject, IDisposable
{
    private bool _disposedValue;
    protected CompositeDisposable Disposables { get; }

    protected BaseViewModel()
    {
        Disposables = new();
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Disposables.Dispose();
            }
            _disposedValue = true;
        }
    }
    
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
