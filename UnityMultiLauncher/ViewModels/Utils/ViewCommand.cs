using System;
using System.Windows.Input;

namespace UnityMultiLauncher.ViewModels.Utils
{
	public class ViewCommand : ICommand
	{
		protected Action<object> execute;
		protected Predicate<object> canExecute;

		public ViewCommand(Action<object> Execute) : this(Execute, null) { }

		public ViewCommand(Action<object> Execute, Predicate<object> CanExecute)
		{
			execute = Execute;
			canExecute = CanExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public virtual bool CanExecute(object parameter)
		{
			return canExecute == null ? true : canExecute(parameter);
		}

		public virtual void Execute(object parameter)
		{
			execute(parameter);
		}
	}
}
