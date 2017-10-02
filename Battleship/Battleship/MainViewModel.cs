using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Battleship.Captains;
using Battleship.Core;
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

            DoubleClickCommand = new RelayCommand(OnDoubleClick);

            _captains = new List<Captain>();
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(ICaptain))))
            {
                _captains.Add(new Captain {CaptainName = mytype.Name, AssemblyQualifiedName = mytype.AssemblyQualifiedName});
            }
        }

        private void OnDoubleClick()
        {
            //string typex = typeof(CaptainMorgan).AssemblyQualifiedName;
            //var theCaptain = (ICaptain)Activator.CreateInstance("Battleship", "CaptainMorgan");
            var type = Type.GetType(SelectedCaptain.AssemblyQualifiedName);
            var myObject = (ICaptain)Activator.CreateInstance(type);
            var window = new CaptainDebugWindow {DataContext = new CaptainDebugViewModel(/*new CaptainMorgan()*/myObject) };
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
