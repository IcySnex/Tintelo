using System.Windows.Input;

namespace Tintelo.iOS.Utils.Extensions;

public static class CommandExtensions
{
	class SilentCommand(
		ICommand command) : ICommand
	{
		public event EventHandler? CanExecuteChanged;
		
		
		public bool CanExecute(object? parameter) =>
			true;

		public void Execute(object? parameter)
		{
			if (command.CanExecute(parameter))
				command.Execute(parameter);
		}
	}
	
	
	extension(ICommand command)
	{
		public ICommand SilentCanExecute() =>
			new SilentCommand(command);
	}
}