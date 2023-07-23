using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private TicTacToeGame _game; // Klasse Binden

        public MainWindow()
        {
            InitializeComponent();

            _game = new TicTacToeGame(); // Klasse instanzieren

            DataContext = _game; // Binden des DataContext

            _game.ResetCommand = new RelayCommand(ResetGame); // Xaml Bindungen
            _game.PlayerMoveCommand = new RelayCommand(_game.ExecutePlayerMove); //Xaml Bindungen
        }

        private void ResetGame()
        {
            _game.ResetGame(null);
        }
    }
}
