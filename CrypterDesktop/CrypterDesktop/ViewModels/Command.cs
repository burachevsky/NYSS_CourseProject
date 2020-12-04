using System;
using System.Windows.Input;

namespace CrypterDesktop.ViewModels
{
    public class Command : ICommand
    {
        private readonly Action action;
        private readonly Func<object, bool> canExecute;

        public Command(Action action, Func<object, bool> canExecute = null)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
