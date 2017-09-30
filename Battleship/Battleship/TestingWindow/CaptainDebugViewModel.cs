using System.Windows.Input;
using Battleship.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Battleship.TestingWindow
{
    public class CaptainDebugViewModel:ViewModelBase
    {
        private readonly ICaptain _captain;
        public CaptainDebugViewModel(ICaptain captain)
        {
            _captain = captain;
            CaptainName = _captain.GetName();
            AIWins = 4;
            AILosses = 6;
            NewGameCommand = new RelayCommand(CreateNewGame);
        }

        public string CaptainName { get; set; }
        public int AIWins { get; set; }
        public int AILosses { get; set; }
        public ICommand NewGameCommand { get; set; }
        private void CreateNewGame()
        {
            _captain.Initialize(1, 2, "Player");
        }


    }
}
