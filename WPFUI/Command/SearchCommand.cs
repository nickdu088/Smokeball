using System;
using System.Windows.Input;

namespace WPFUI.Command
{
    public class SearchCommand : ICommand
    {
        private readonly Action _action;
        public SearchCommand(Action action)
        {
            _action = action;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
