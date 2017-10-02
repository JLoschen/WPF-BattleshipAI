using System.Collections.Generic;
using System.Windows.Input;
using Battleship.Captains;
using Battleship.TestingWindow;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Battleship
{
    public class MainViewModel: ViewModelBase
    {
        public MainViewModel()
        {
            _opponents = new List<string> {"Black Beard", "Red Beard", "Dread Pirate Roberts"};
            _numberOfMatchesOptions = new List<int> {500, 1000, 5000, 10000, 50000, 250000, 1000000};
            _captains = new List<Captain>
            {
                new Captain {CaptainName = "Black Beard", Score = 999},
                new Captain {CaptainName = "Dread Pirate Roberts", Score = 111},
                new Captain {CaptainName = "Red Beard", Score = 222}
            };

            DoubleClickCommand = new RelayCommand(OnDoubleClick);
        }

        private void OnDoubleClick()
        {
            var window = new CaptainDebugWindow {DataContext = new CaptainDebugViewModel(new /*CaptainLoco()*/CaptainMorgan())};
            window.Show();
        }

        public List<string> Opponents 
        { 
            get{ return _opponents; } 
            set{ Set(ref _opponents, value); } 
        } 
        private List<string> _opponents;

        public List<int> NumberOfMatchesOptions 
        { 
            get{ return _numberOfMatchesOptions; } 
            set{ Set(ref _numberOfMatchesOptions, value); } 
        } 
        private List<int> _numberOfMatchesOptions;

        public List<Captain> Captains 
        { 
            get{ return _captains; } 
            set{ Set(nameof(Captains),ref _captains, value); } 
        } 
        private List<Captain> _captains;

        public Captain SelectedCaptain 
        { 
            get{ return _selectedCaptain; } 
            set{ Set(ref _selectedCaptain, value); } 
        } 
        private Captain _selectedCaptain;

        public ICommand DoubleClickCommand { get; set; }
    }
}
