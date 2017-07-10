using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ClueDo.Utilities
{
    /// <summary>
    /// Copied from the MVVM article in MSDN magazine. Read more about the MVVM pattern there.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        /// <summary>
        /// Constructor with can execute predicate
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <author>PD-101105</author>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Constructor with can execute predicate
        /// </summary>
        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            if (canExecute != null)
                _canExecute = p => canExecute();
        }

        /// <summary>
        /// Constructor with can execute predicate
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <author>YP-120820</author>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = p => execute();
            if (canExecute != null)
                _canExecute = p => canExecute();
        }

        /// <summary>
        /// Check if the given parameter can be executer by the command
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <author>PD-101105</author>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter = null)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        /// <summary>
        /// Handle updates to CanExecute
        /// </summary>
        /// <author>PD-101105</author>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="parameter"></param>
        /// <author>PD-101105</author>
        public void Execute(object parameter = null)
        {
            _execute((T)parameter);
        }
    }

    /// <summary>
    /// Copied from the MVVM article in MSDN magazine. Read more about the MVVM pattern there.
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute"></param>
        /// <author>YP-130206</author>
        public RelayCommand(Action<object> execute)
            : base(execute)
        {
        }

        /// <summary>
        /// Constructor with can execute predicate
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <author>YP-130206</author>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute"></param>
        /// <author>YP-130206</author>
        public RelayCommand(Action execute)
            : base(execute)
        {
        }

        /// <summary>
        /// Constructor with can execute predicate
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <author>YP-130206</author>
        public RelayCommand(Action execute, Func<bool> canExecute)
            : base(execute, canExecute)
        {
        }
    }
}