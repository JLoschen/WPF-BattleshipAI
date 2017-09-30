using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Battleship.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Battleship.TestingWindow
{
    public class CaptainDebugViewModel:ViewModelBase
    {
        private readonly ICaptain _captain;

        private int[,] _gameBoard;
        
        public CaptainDebugViewModel(ICaptain captain)
        {
            _captain = captain;
            CaptainName = _captain.GetName();
            AIWins = 4;
            AILosses = 6;
            NewGameCommand = new RelayCommand(CreateNewGame);
            CellClickedCommand = new RelayCommand<int>(CellClicked);
            GameState = GameState.Waiting;
            GameBoard2 = GetBlankBoard2();
            PlayerShipVisibility = new ObservableCollection<bool> {false, false, false, false, false};
        }

        private void CellClicked(int cell)
        {
            //if (GameState == GameState.Waiting) return;
            switch (GameState)
            {
                case GameState.HumanPlayerPlacingPatrol:
                    GameState = GameState.HumanPlayerPlacingDestroyer;
                    PlayerShipVisibility[Constants.Destroyer] = true;
                    break;
                case GameState.HumanPlayerPlacingDestroyer:
                    GameState = GameState.HumanPlayerPlacingSubmarine;
                    PlayerShipVisibility[Constants.Submarine] = true;
                    break;
                case GameState.HumanPlayerPlacingSubmarine:
                    GameState = GameState.HumanPlayerPlacingBattleship;
                    PlayerShipVisibility[Constants.Battleship] = true;
                    break;
                case GameState.HumanPlayerPlacingBattleship:
                    GameState = GameState.HumanPlayerPlacingAircraftCarrier;
                    PlayerShipVisibility[Constants.AircraftCarrier] = true;
                    break;
                case GameState.HumanPlayerPlacingAircraftCarrier:
                    GameState = GameState.HumansTurn;
                    break;
            }
            //var x = cell % 10;
            //var y = cell / 10;

            //Grid.SetRow();
        }

        public string CaptainName { get; set; }

        public ICommand NewGameCommand { get; set; }
        private void CreateNewGame()
        {
            
            _gameBoard = GetBlankBoard();

            GameState = GameState.HumanPlayerPlacingPatrol;
            //WaitForHumanPlayerToSetFleet();

            //IsPlayersTurn = true;
            //EnterGameLoop();
        }

        private void WaitForHumanPlayerToSetFleet()
        {
            GameState = GameState.HumanPlayerPlacingPatrol;


        }
        
        public GameState GameState { get; set; }

        private async void EnterGameLoop()
        {
            IsGameInProgress = true;
            var numAttacks = 0;
            
            _captain.Initialize(1, 2, "Player");
            while (IsGameInProgress)
            {

                var attack =_captain.MakeAttack();

                AiFleet = _captain.GetFleet();

                PlayerFleet = _captain.GetFleet();

                GameBoard2[attack.X][attack.Y] = 106;
                numAttacks++;
                IsGameInProgress = numAttacks < 20;
                await Task.Delay(TimeSpan.FromMilliseconds(2000)); //wait 2 seconds between shots
            }
        }

        private int[,] GetBlankBoard()
        {
            var board = new[,] 
            { 
                {0,0,0,0,0,0,0,0,0,0}, //0
                {0,0,0,0,0,0,0,0,0,0}, //1
                {0,0,0,0,0,0,0,0,0,0}, //2
                {0,0,0,0,0,0,0,0,0,0}, //3
                {0,0,0,0,0,0,0,0,0,0}, //4
                {0,0,0,0,0,0,0,0,0,0}, //5
                {0,0,0,0,0,0,0,0,0,0}, //6
                {0,0,0,0,0,0,0,0,0,0}, //7
                {0,0,0,0,0,0,0,0,0,0}, //8
                {0,0,0,0,0,0,0,0,0,0}, //9 
            };
            return board;
        }

        private ObservableCollection<ObservableCollection<int>> GetBlankBoard2()
        {
            var row0 = new ObservableCollection<int>(GetBoardRow());
            var row1 = new ObservableCollection<int>(GetBoardRow());
            var row2 = new ObservableCollection<int>(GetBoardRow());
            var row3 = new ObservableCollection<int>(GetBoardRow());
            var row4 = new ObservableCollection<int>(GetBoardRow());
            var row5 = new ObservableCollection<int>(GetBoardRow());
            var row6 = new ObservableCollection<int>(GetBoardRow());
            var row7 = new ObservableCollection<int>(GetBoardRow());
            var row8 = new ObservableCollection<int>(GetBoardRow());
            var row9 = new ObservableCollection<int>(GetBoardRow());

            return new ObservableCollection<ObservableCollection<int>> {row0, row1, row2, row3, row4, row5, row6, row7, row8, row9};
        }
        
        private int[] GetBoardRow()
        {
            return new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        public Fleet AiFleet 
        { 
            get{ return _aiFleet; } 
            set{ Set(ref _aiFleet, value); } 
        } 
        private Fleet _aiFleet;

        public Fleet PlayerFleet
        {
            get { return _playerFleet; }
            set { Set(ref _playerFleet, value); }
        }
        private Fleet _playerFleet;

        //I'll be surprised if this works
        public ObservableCollection<ObservableCollection<int>> GameBoard2
        {
            get { return _gameBoard2; }
            set { Set(nameof(GameBoard2), ref _gameBoard2, value); }
        }
        private ObservableCollection<ObservableCollection<int>> _gameBoard2;

        public bool IsGameInProgress
        {
            get { return _isGameInProgress; }
            set { Set(ref _isGameInProgress, value); }
        }
        private bool _isGameInProgress;

        public bool IsPlayersTurn
        {
            get { return _isPlayersTurn; }
            set { Set(ref _isPlayersTurn, value); }
        }
        private bool _isPlayersTurn;

        public bool IsSettingVertical 
        { 
            get{ return _isSettingVertical; } 
            set{ Set(ref _isSettingVertical, value); } 
        } 
        private bool _isSettingVertical;

        public int AIWins
        {
            get { return _aIWins; }
            set { Set(nameof(AIWins), ref _aIWins, value); }
        }
        private int _aIWins;

        public int AILosses
        {
            get { return _aILosses; }
            set { Set(ref _aILosses, value); }
        }
        private int _aILosses;

        public ObservableCollection<bool> PlayerShipVisibility { get; set; }

        public ICommand CellClickedCommand { get; set; }
    }
}
