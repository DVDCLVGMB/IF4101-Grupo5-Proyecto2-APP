using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void Raise([CallerMemberName] string prop = null!) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}