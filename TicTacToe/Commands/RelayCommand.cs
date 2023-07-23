using System;
using System.Windows.Input;

namespace TicTacToe
{
    public class RelayCommand : ICommand
    {
        public readonly Action<object> execute;
        public readonly Predicate<object> canExecute;
        private readonly Action resetGame; // Korrekte Signatur für resetGame

        public RelayCommand(Action resetGame)
        {
            this.resetGame = resetGame;
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (resetGame != null)
            {
                resetGame.Invoke(); // Rufen Sie den resetGame-Delegaten auf, wenn er nicht null ist
            }
            else
            {
                execute?.Invoke(parameter); // Stellen Sie sicher, dass execute nicht null ist
            }
        }
    }
}

