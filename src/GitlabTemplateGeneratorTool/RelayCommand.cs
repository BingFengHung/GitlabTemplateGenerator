using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GitlabTemplateGeneratorTool
{
    class RelayCommand : ICommand
    {
        public Action<object> _execute;
        public Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, (obj) => true)
        {

        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
}
