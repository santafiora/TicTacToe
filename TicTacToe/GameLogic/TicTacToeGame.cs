using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TicTacToe
{
    public class TicTacToeGame : INotifyPropertyChanged
    {
        private readonly List<ButtonViewModel> buttons;
        private readonly Random rand = new Random();
        private Player currentPlayer;
        private int playerWins;
        private int computerWins;
        private int _draws;
        private int maxDepth = 0; // Rechentiefe begrenzen

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand PlayerMoveCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        public int PlayerWins
        {
            get => playerWins;
            set
            {
                if (value != playerWins)
                {
                    playerWins = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayerWins)));
                }
            }
        }

        public void ExecutePlayerMove(ButtonViewModel button)
        {
            if (button != null && button.IsEnabled)
            {
                PlayerMove(button);
            }
        }

        public int ComputerWins
        {
            get => computerWins;
            set
            {
                if (value != computerWins)
                {
                    computerWins = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComputerWins)));
                }
            }
        }

        public int Draws
        {
            get => _draws;
            set
            {
                if (value != _draws)
                {
                    _draws = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Draws)));
                }
            }
        }

        public int MaxDepth
        {
            get => maxDepth;
            set
            {
                if (value != maxDepth)
                {
                    maxDepth = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxDepth)));
                }
            }
        }
        public TicTacToeGame()
        {
            buttons = new List<ButtonViewModel>();
            for (int i = 0; i < 9; i++)
            {
                buttons.Add(new ButtonViewModel());
            }
            currentPlayer = Player.X;
        }

        public void ExecutePlayerMove(object parameter)
        {
            if (parameter is ButtonViewModel button && button.IsEnabled)
            {
                PlayerMove(button);
            }
        }

        public IEnumerable<ButtonViewModel> Buttons => buttons;

        public void PlayerMove(ButtonViewModel button)
        {
            button.Text = currentPlayer.ToString();
            button.IsEnabled = false;
            // button.Background = System.Windows.Media.Brushes.Cyan;
            button.IsAvailable = false;

            Check();

            if (currentPlayer == Player.X)
            {
                currentPlayer = Player.O;
                AImove();
            }
        }

        // AI MOVE ALPHA BETA
        public void AImove()
        {
            if (buttons.Count > 0)
            {
                var availableButtons = buttons.Where(b => b.IsAvailable).ToList();
                if (availableButtons.Count > 0)
                {
                    int bestScore = int.MinValue;
                    ButtonViewModel bestMove = null;

                    foreach (var button in availableButtons)
                    {
                        button.IsEnabled = false;
                        currentPlayer = Player.O;
                        button.Text = currentPlayer.ToString();
                        button.IsAvailable = false;

                        int score = MinimaxAlphaBeta(buttons, 0, int.MinValue, int.MaxValue, false);

                        button.IsEnabled = true;
                        button.Text = string.Empty;
                        button.IsAvailable = true;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = button;
                        }
                    }

                    if (bestMove != null)
                    {
                        bestMove.IsEnabled = false;
                        currentPlayer = Player.O;
                        bestMove.Text = currentPlayer.ToString();
                        bestMove.IsAvailable = false;

                        Check();

                        if (currentPlayer == Player.O)
                        {
                            currentPlayer = Player.X;
                        }
                    }
                }
            }
        }
        public int MinimaxAlphaBeta(List<ButtonViewModel> board, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            int score = Evaluate(board);
            if (score != 0 || depth >= maxDepth)
            {
                return score;
            }

            if (buttons.All(button => !button.IsAvailable))
            {
                return 0; // Unentschieden
            }

            if (isMaximizingPlayer)
            {
                int maxScore = int.MinValue;
                foreach (var button in board.Where(b => b.IsAvailable))
                {
                    button.IsEnabled = false;
                    currentPlayer = Player.O;
                    button.Text = currentPlayer.ToString();
                    button.IsAvailable = false;

                    int currentScore = MinimaxAlphaBeta(board, depth + 1, alpha, beta, false);

                    button.IsEnabled = true;
                    button.Text = string.Empty;
                    button.IsAvailable = true;

                    maxScore = Math.Max(maxScore, currentScore);
                    alpha = Math.Max(alpha, maxScore);

                    if (beta <= alpha)
                    {
                        break; // Beta-Cutoff
                    }
                }

                return maxScore;
            }
            else
            {
                int minScore = int.MaxValue;
                foreach (var button in board.Where(b => b.IsAvailable))
                {
                    button.IsEnabled = false;
                    currentPlayer = Player.X;
                    button.Text = currentPlayer.ToString();
                    button.IsAvailable = false;

                    int currentScore = MinimaxAlphaBeta(board, depth + 1, alpha, beta, true);

                    button.IsEnabled = true;
                    button.Text = string.Empty;
                    button.IsAvailable = true;

                    minScore = Math.Min(minScore, currentScore);
                    beta = Math.Min(beta, minScore);

                    if (beta <= alpha)
                    {
                        break; // Alpha-Cutoff
                    }
                }

                return minScore;
            }
        }
        public int Evaluate(List<ButtonViewModel> board)
        {
            var lines = new[]
            {
       new[] { 0, 1, 2 },
       new[] { 3, 4, 5 },
       new[] { 6, 7, 8 },
       new[] { 0, 3, 6 },
       new[] { 1, 4, 7 },
       new[] { 2, 5, 8 },
       new[] { 0, 4, 8 },
       new[] { 2, 4, 6 }
   };

            foreach (var line in lines)
            {
                if (line.All(index => board[index].Text == "X"))
                {
                    return -1; // Minimizing player wins
                }
                else if (line.All(index => board[index].Text == "O"))
                {
                    return 1; // Maximizing player wins
                }
            }

            return 0; // Unentschieden
        }
        public void NewGame()
        {
            foreach (var button in buttons)
            {
                button.IsEnabled = true;
                button.Text = string.Empty;
                button.IsAvailable = true;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Buttons)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(playerWins)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComputerWins)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Draws)));

        }

        public void ResetGame(object obj)
        {
            currentPlayer = Player.X;
            NewGame();
        }

        public void Check()
        {
            var lines = new[]
            {
                new[] { 0, 1, 2 },
                new[] { 3, 4, 5 },
                new[] { 6, 7, 8 },
                new[] { 0, 3, 6 },
                new[] { 1, 4, 7 },
                new[] { 2, 5, 8 },
                new[] { 0, 4, 8 },
                new[] { 2, 4, 6 }
            };

            foreach (var line in lines)
            {
                if (line.All(index => buttons[index].Text == "X"))
                {
                    MessageBox.Show("Player wins!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                    playerWins++;

                    NewGame();

                    return;
                }
                else if (line.All(index => buttons[index].Text == "O"))
                {
                    MessageBox.Show("Computer wins!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                    computerWins++;

                    NewGame();

                    return;
                }
            }

            // Überprüfen auf Unentschieden
            if (buttons.All(button => !button.IsAvailable))
            {
                MessageBox.Show("It's a draw!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                _draws++;

                NewGame();
            }

        }
    }
}

