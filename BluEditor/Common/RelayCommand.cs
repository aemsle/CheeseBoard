using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

namespace BluEditor
{
    internal class RelayCommand<T> : ICommand
    {
        private readonly Action<T> m_execute;
        private readonly Predicate<T> m_canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            m_execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute?.Invoke((T)parameter) ?? true;
        }

        public RelayCommand(Action<T> in_execute, Predicate<T> in_canExecute = null)
        {
            m_execute = in_execute ?? throw new ArgumentNullException(nameof(in_execute));
            m_canExecute = in_canExecute;
        }
    }
}