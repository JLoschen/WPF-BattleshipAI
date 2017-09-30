using System.Collections.ObjectModel;
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
            GameState = GameState.Waiting;
        }

        public string CaptainName { get; set; }
        public int AIWins { get; set; }
        public int AILosses { get; set; }
        public ICommand NewGameCommand { get; set; }
        private void CreateNewGame()
        {
            _captain.Initialize(1, 2, "Player");
            _gameBoard = GetBlankBoard();

            WaitForHumanPlayerToSetFleet();
            
            //IsPlayersTurn = true;
            //EnterGameLoop();
        }

        private void WaitForHumanPlayerToSetFleet()
        {
            GameState = GameState.HumanPlayerPlacingPatrol;


        }

        //I'll be surprised if this works
        public ObservableCollection<ObservableCollection<int>> GameBoard2 
        { 
            get{ return _gameBoard2; } 
            set{ Set(nameof(GameBoard2),ref _gameBoard2, value); } 
        } 
        private ObservableCollection<ObservableCollection<int>> _gameBoard2;


        public GameState GameState { get; set; }

        private void EnterGameLoop()
        {
            IsGameInProgress = true;

            while (IsGameInProgress)
            {
                var attack =_captain.MakeAttack();
            }

        }

        public bool IsGameInProgress 
        { 
            get{ return _isGameInProgress; } 
            set{ Set(ref _isGameInProgress, value); } 
        } 
        private bool _isGameInProgress;

        public bool IsPlayersTurn 
        { 
            get{ return _isPlayersTurn; } 
            set{ Set(ref _isPlayersTurn, value); } 
        } 
        private bool _isPlayersTurn;

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
            return new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        }
    }
}
