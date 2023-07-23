using System.ComponentModel;

namespace TicTacToe
{

    public class ButtonViewModel : INotifyPropertyChanged
    {
        private string _text;
        private bool _isEnabled = true;
        private System.Windows.Media.Brush _background = System.Windows.Media.Brushes.BlanchedAlmond;
        private bool _isAvailable = true;

        /*   public ButtonViewModel()
           {
           }*/

        public int Index { get; }

        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    RaisePropertyChanged(nameof(Text));
                }
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    RaisePropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public System.Windows.Media.Brush Background
        {
            get => _background;
            set
            {
                if (value != _background)
                {
                    _background = value;
                    RaisePropertyChanged(nameof(Background));
                }
            }
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set
            {
                if (value != _isAvailable)
                {
                    _isAvailable = value;
                    RaisePropertyChanged(nameof(IsAvailable));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

