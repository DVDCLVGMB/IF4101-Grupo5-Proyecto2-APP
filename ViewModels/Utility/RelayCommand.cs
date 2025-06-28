using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?> _canExecute;
    public RelayCommand(Action<object?> exec, Predicate<object?> can = null!)
    {
        _execute = exec;
        _canExecute = can ?? (_ => true);
    }
    public bool CanExecute(object? p) => _canExecute(p);
    public void Execute(object? p) => _execute(p);
    public event EventHandler? CanExecuteChanged;
    public void RaiseCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}