using System;
using System.Windows.Input;

namespace Mail.Client.WPF
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _onExecute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter = null)
        {
            return _canExecute == null || CanExecute(parameter);
        }

        public void Execute(object parameter = null)
        {
            _onExecute(parameter);
        }

        public DelegateCommand(Action<object> onExecute, Func<object, bool> canExecute = null)
        {
            _canExecute = canExecute;
            _onExecute = onExecute;
        }
    }
}