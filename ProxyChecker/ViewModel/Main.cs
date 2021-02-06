using System.ComponentModel;

namespace ProxyChecker.ViewModel
{
    public class Main : INotifyPropertyChanged
    {
        private int _error;

        private int _good;

        private int _left;

        private int _speedSlider = 200;

        private string _targetTextBox = "https://www.google.com/";

        private int _timeout;

        private int _timeoutSlider = 1000;

        private int _total;

        public int Total
        {
            get => _total;
            set
            {
                if (value != _total)
                {
                    _total = value;
                    OnPropertyChanged("Total");
                }
            }
        }

        public int Left
        {
            get => _left;
            set
            {
                if (value != _left)
                {
                    _left = value;
                    OnPropertyChanged("Left");
                }
            }
        }

        public int Good
        {
            get => _good;
            set
            {
                if (value != _good)
                {
                    _good = value;
                    OnPropertyChanged("Good");
                }
            }
        }

        public int Timeout
        {
            get => _timeout;
            set
            {
                if (value != _timeout)
                {
                    _timeout = value;
                    OnPropertyChanged("Timeout");
                }
            }
        }

        public int Error
        {
            get => _error;
            set
            {
                if (value != _error)
                {
                    _error = value;
                    OnPropertyChanged("Error");
                }
            }
        }

        public int TimeoutSlider
        {
            get => _timeoutSlider;
            set
            {
                if (value != _timeoutSlider)
                {
                    _timeoutSlider = value;
                    OnPropertyChanged("TimeoutSlider");
                }
            }
        }

        public int SpeedSlider
        {
            get => _speedSlider;
            set
            {
                if (value != _speedSlider)
                {
                    _speedSlider = value;
                    OnPropertyChanged("SpeedSlider");
                }
            }
        }

        public string TargetTextBox
        {
            get => _targetTextBox;
            set
            {
                if (value != _targetTextBox)
                {
                    _targetTextBox = value;
                    OnPropertyChanged("TargetTextBox");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ResetData()
        {
            Good = 0;
            Total = 0;
            Left = 0;
            Timeout = 0;
            Error = 0;
        }
    }
}