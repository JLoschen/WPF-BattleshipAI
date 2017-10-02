using System.Collections.ObjectModel;
using System.Windows;
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
            NewGameCommand = new RelayCommand(CreateNewGame);
            PlayerCellClickedCommand = new RelayCommand<int>(PlayerCellClicked);
            CellClickedCommand = new RelayCommand<int>(CellClicked);
            RightClickCommand = new RelayCommand<int>(CellRightClicked);
        }

        public Fleet CreateFleet()
        {
            _captain.Initialize(1, 2, "Player");
            AiFleet = _captain.GetFleet();
            SetAiShipVisibility();
                
            GameState = GameState.HumansTurn;
            return AiFleet;
        }

        private void CellRightClicked(int cell)
        {
            switch (GameState)
            {
                case GameState.HumanPlayerPlacingPatrol:
                    PlayerShipVisibility[Constants.PatrolBoat] = !PlayerShipVisibility[Constants.PatrolBoat];
                    PlayerShipVerticalVisibility[Constants.PatrolBoat] = !PlayerShipVerticalVisibility[Constants.PatrolBoat];
                    break;
                case GameState.HumanPlayerPlacingDestroyer:
                    PlayerShipVisibility[Constants.Destroyer] = !PlayerShipVisibility[Constants.Destroyer];
                    PlayerShipVerticalVisibility[Constants.Destroyer] = !PlayerShipVerticalVisibility[Constants.Destroyer];
                    break;
                case GameState.HumanPlayerPlacingSubmarine:
                    PlayerShipVisibility[Constants.Submarine] = !PlayerShipVisibility[Constants.Submarine];
                    PlayerShipVerticalVisibility[Constants.Submarine] = !PlayerShipVerticalVisibility[Constants.Submarine];
                    break;
                case GameState.HumanPlayerPlacingBattleship:
                    PlayerShipVisibility[Constants.Battleship] = !PlayerShipVisibility[Constants.Battleship];
                    PlayerShipVerticalVisibility[Constants.Battleship] = !PlayerShipVerticalVisibility[Constants.Battleship];
                    break;
                case GameState.HumanPlayerPlacingAircraftCarrier:
                    PlayerShipVisibility[Constants.AircraftCarrier] = !PlayerShipVisibility[Constants.AircraftCarrier];
                    PlayerShipVerticalVisibility[Constants.AircraftCarrier] = !PlayerShipVerticalVisibility[Constants.AircraftCarrier];
                    break;
            }
        }

        private void PlayerCellClicked(int cell)
        {
            if(GameState != GameState.HumansTurn) return;

            var x = cell % 10;
            var y = cell / 10;
            var attackedCell = new Coordinate(x, y);
            _captain.OpponentAttack(attackedCell);
            var result = AiFleet.Attacked(attackedCell);
            PlayerGameBoard[x][y] = result;

            if (result == Constants.Defeated)
            {
                MessageBox.Show("You win!");
                AILosses++;
                GameState = GameState.Waiting;
                return;
            }
            GameState = GameState.AIsTurn;

            var aiAttack = _captain.MakeAttack();
            result = PlayerFleet.Attacked(aiAttack);
            _captain.ResultOfAttack(result);

            AIGameBoard[aiAttack.X][aiAttack.Y] = result;

            if (result == Constants.Defeated)
            {
                MessageBox.Show("You lose :(");
                AIWins++;
                GameState = GameState.Waiting;
                return;
            }
            GameState = GameState.HumansTurn;
        }

        private void CellClicked(int cell)
        {
            var coord = new Coordinate(cell % 10, cell / 10);
            switch (GameState)
            {
                case GameState.HumanPlayerPlacingPatrol:
                    var ptVertical = PlayerShipVisibility[Constants.PatrolBoat];
                    if (PlayerFleet.PlaceShip(coord, ptVertical, Constants.PatrolBoat))
                    {
                        GameState = GameState.HumanPlayerPlacingDestroyer;
                        PlayerShipVisibility[Constants.Destroyer] = true;
                    }
                    break;
                case GameState.HumanPlayerPlacingDestroyer:
                    var destVertical = PlayerShipVisibility[Constants.Destroyer];
                    if (PlayerFleet.PlaceShip(coord, destVertical, Constants.Destroyer))
                    {
                        GameState = GameState.HumanPlayerPlacingSubmarine;
                        PlayerShipVisibility[Constants.Submarine] = true;
                    }
                    break;
                case GameState.HumanPlayerPlacingSubmarine:
                    var subVertical = PlayerShipVisibility[Constants.Submarine];
                    
                    if (PlayerFleet.PlaceShip(coord, subVertical, Constants.Submarine))
                    {
                        GameState = GameState.HumanPlayerPlacingBattleship;
                        PlayerShipVisibility[Constants.Battleship] = true;
                    }
                        
                    break;
                case GameState.HumanPlayerPlacingBattleship:
                    var batVertical = PlayerShipVisibility[Constants.Battleship];
                    if (PlayerFleet.PlaceShip(coord, batVertical, Constants.Battleship))
                    {
                        GameState = GameState.HumanPlayerPlacingAircraftCarrier;
                        PlayerShipVisibility[Constants.AircraftCarrier] = true;
                    }
                    break;
                case GameState.HumanPlayerPlacingAircraftCarrier:
                    var airVertical = PlayerShipVisibility[Constants.AircraftCarrier];
                    if (PlayerFleet.PlaceShip(coord, airVertical, Constants.AircraftCarrier))
                    {
                        GameState = GameState.ReadyWaitingToStart;
                    }
                        
                    break;
            }
            //var x = cell % 10;
            //var y = cell / 10;

            //Grid.SetRow();
        }

        public string CaptainName => _captain?.GetName();

        public ICommand NewGameCommand { get; set; }
        private void CreateNewGame()
        {
            AIGameBoard = GetBlankBoard();
            PlayerGameBoard = GetBlankBoard();
            _playerFleet = new Fleet();

            for (int i = 0; i < 5; i++)
            {
                AIShipVerticalVisibility[i] = false;
                AIShipHorizontalVisibility[i] = false;
                PlayerShipVisibility[i] = false;
                PlayerShipVerticalVisibility[i] = false;
            }
            
            GameState = GameState.HumanPlayerPlacingPatrol;
            PlayerShipVisibility[Constants.PatrolBoat] = true;
        }
        
        public GameState GameState 
        { 
            get{ return _gameState; }
            set
            {
                if (Set(ref _gameState, value))
                {
                    switch (_gameState)
                    {
                        case GameState.HumanPlayerPlacingPatrol:
                            CurrentShipBeingPositioned = Constants.PatrolBoat;
                            break;
                        case GameState.HumanPlayerPlacingDestroyer:
                            CurrentShipBeingPositioned = Constants.Destroyer;
                            break;
                        case GameState.HumanPlayerPlacingSubmarine:
                            CurrentShipBeingPositioned = Constants.Submarine;
                            break;
                        case GameState.HumanPlayerPlacingBattleship:
                            CurrentShipBeingPositioned = Constants.Battleship;
                            break;
                        case GameState.HumanPlayerPlacingAircraftCarrier:
                            CurrentShipBeingPositioned = Constants.AircraftCarrier;
                            break;
                    }
                }
            } 
        } 
        private GameState _gameState = GameState.Waiting;

        private ObservableCollection<ObservableCollection<int>> GetBlankBoard()
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
        private Fleet _aiFleet = new Fleet();

        public Fleet PlayerFleet
        {
            get { return _playerFleet; }
            set { Set(ref _playerFleet, value); }
        }
        private Fleet _playerFleet = new Fleet();
        
        public int CurrentShipBeingPositioned 
        { 
            get{ return _currentShipBeingPositioned; } 
            set{ Set(ref _currentShipBeingPositioned, value); } 
        } 
        private int _currentShipBeingPositioned;

        //I'll be surprised if this works
        public ObservableCollection<ObservableCollection<int>> AIGameBoard
        {
            get { return _aiGameBoard; }
            set { Set(nameof(AIGameBoard), ref _aiGameBoard, value); }
        }
        private ObservableCollection<ObservableCollection<int>> _aiGameBoard;

        public ObservableCollection<ObservableCollection<int>> PlayerGameBoard
        {
            get { return _playerGameBoard; }
            set { Set(nameof(PlayerGameBoard), ref _playerGameBoard, value); }
        }
        private ObservableCollection<ObservableCollection<int>> _playerGameBoard;

        public bool IsGameInProgress
        {
            get { return _isGameInProgress; }
            set { Set(ref _isGameInProgress, value); }
        }
        private bool _isGameInProgress;

        public bool ShowAiShips 
        { 
            get{ return _showAiShips; } 
            set
            {
                if (Set(ref _showAiShips, value))
                {
                    SetAiShipVisibility();
                }
            } 
        }

        private void SetAiShipVisibility()
        {
            if (ShowAiShips)
            {
                var fleet = AiFleet.GetFleet();
                for (var ship = 0; ship < 5; ship++)
                {
                    var s = fleet[ship];
                    if (s == null) continue;
                    if (s.IsVertical)
                    {
                        AIShipVerticalVisibility[ship] = true;
                    }
                    else
                    {
                        AIShipHorizontalVisibility[ship] = true;
                    }
                }
            }
            else
            {
                for (var ship = 0; ship < 5; ship++)
                {
                    AIShipHorizontalVisibility[ship] = false;
                    AIShipVerticalVisibility[ship] = false;
                }
            }
        }

        private bool _showAiShips;
        
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

        public ObservableCollection<bool> PlayerShipVisibility { get; set; } = new ObservableCollection<bool> { false, false, false, false, false };
        public ObservableCollection<bool> PlayerShipVerticalVisibility { get; set; } = new ObservableCollection<bool> { false, false, false, false, false };
        public ObservableCollection<bool> AIShipHorizontalVisibility { get; set; } = new ObservableCollection<bool> { false, false, false, false, false };
        public ObservableCollection<bool> AIShipVerticalVisibility { get; set; } = new ObservableCollection<bool> { false, false, false, false, false };

        public ICommand CellClickedCommand { get; set; }
        public ICommand PlayerCellClickedCommand { get; set; }
        public ICommand RightClickCommand { get; set; }
        public ICommand StartGameCommand { get; set; }
    }
}
