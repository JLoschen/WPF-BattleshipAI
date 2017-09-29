using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace Battleship
{
    public class MainViewModel: ViewModelBase
    {
        public MainViewModel()
        {
            _opponents = new List<string> {"Black Beard", "Red Beard", "Dread Pirate Roberts"};
            _numberOfMatchesOptions = new List<int> {500, 1000, 5000, 10000, 50000, 250000, 1000000};
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


    }
}
