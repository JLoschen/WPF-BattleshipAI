using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Battleship.Core;
using Battleship.TestingWindow;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Battleship.Main
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DoubleClickCommand = new RelayCommand(OnDoubleClick);
            RunCompetitionCommand = new RelayCommand(RunCompetition);
            ResetCommand = new RelayCommand(Reset);
            StopCommand = new RelayCommand(Stop);

            _captains = new List<Captain>();
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(ICaptain))))
            {
                var captain = new Captain
                {
                    CaptainName = mytype.Name,
                    AssemblyQualifiedName = mytype.AssemblyQualifiedName,
                    CaptainStatistics = new Dictionary<string, CaptainStatistics>(),
                    AllAttacks = new int[100],
                    AttackHeat = new float[100],
                    PlacementHeat = new float[100],
                    AllPlacements = new int[100],
                    ShipPlacements = new int[5, 10, 10]
                };
                _captains.Add(captain);
            }
        }

        private void Stop()
        {
            StopCompetition = true;
        }

        private void Reset()
        {
            foreach (var captain in Captains)
            {
                captain.Score = 0;
            }
            CurrentMatchProgress = 0;
            MatchProgress = 0;
        }

        private async void RunCompetition()
        {
            var captainsToRun = Captains.Where(c => c.IsSelected).ToList();

            var numCaptains = captainsToRun.Count;
            // Compute the number of total rounds for keeping track of progress
            int numberofrounds = numCaptains * numCaptains - numCaptains;

            int counter = 0;
            foreach (var captain in captainsToRun)
            {
                var captainOpponents = captainsToRun.Where(c => c.AssemblyQualifiedName != captain.AssemblyQualifiedName);
                foreach (var captain2 in captainOpponents)
                {
                    if (StopCompetition) return;
                    if (await BattleCaptains(captain, captain2, captainsToRun.Count))
                    {
                        counter++;
                    }

                    MatchProgress =  100 * counter / numberofrounds;
                }
            }
        }

        private async Task<bool> BattleCaptains(Captain captain1, Captain captain2, int numCaptains)
        {
            // reset the progress bar
            StopCompetition = false;
            CurrentMatchProgress = 0;

            var captainOne = captain1.GetNewCaptain();
            var captainTwo = captain2.GetNewCaptain();
            // Get the names of the two captains
            string nameOne = captain1.CaptainName;
            string nameTwo = captain2.CaptainName;

            if (!captain1.CaptainStatistics.ContainsKey(nameTwo))//if haven't played before
            {
                captain1.CaptainStatistics.Add(new KeyValuePair<string, CaptainStatistics>(nameTwo, new CaptainStatistics()));
                captain2.CaptainStatistics.Add(new KeyValuePair<string, CaptainStatistics>(nameOne, new CaptainStatistics()));
            }

            // Remember their scores (how many matches they have won).
            int scoreOne = 0, scoreTwo = 0;

            var halfNumberOfMatches = SelectedNumberOfMatches / 2;
            for (int i = 0; i < halfNumberOfMatches; i++)
            {
                captainOne.Initialize(SelectedNumberOfMatches, numCaptains, nameTwo);
                captainTwo.Initialize(SelectedNumberOfMatches, numCaptains, nameOne);

                if (i % 2 == 0)
                {
                    //captain 1 goes first
                    if (RunGame(captain1, captain2 ))
                    {
                        scoreOne++;
                    }
                    else
                    {
                        scoreTwo++;
                    }

                }
                else
                {
                    //captain 2 goes first
                    if (RunGame(captain2, captain1))
                    {
                        scoreTwo++;
                    }
                    else
                    {
                        scoreOne++;
                    }
                }

                // Has the user requested that we stop?
                if (StopCompetition) return false;

                if (i % 5 == 0)
                {
                    //update progress bar   
                    CurrentMatchProgress = 100 * (i + 1) / halfNumberOfMatches + 1;
                    await Task.Delay(TimeSpan.FromMilliseconds(1));//give up thread control so UI can render

                    //update score totals every 500 matches
                    if (i % 500 == 0 && i > 0)
                    {
                        captain2.Score += scoreTwo;
                        captain1.Score += scoreOne;

                        scoreOne = scoreTwo = 0;
                    }
                }
            }
            captain2.Score += scoreTwo;
            captain1.Score += scoreOne;

            captain1.UpdateGui();
            captain2.UpdateGui();
            
            return true;
        }

        private bool RunGame(Captain captain1, Captain captain2)
        {
            // Record his ship placement choices
            Fleet fleetone = captain1.CaptainAI.GetFleet();
            foreach (var ship in fleetone.GetFleet())
            {
                foreach (var coordinate in ship.GetCoordinates())
                {
                    captain1.ShipPlacements[ship.Model, coordinate.X, coordinate.Y]++;
                }
            }

            // Record her ship placement choices
            Fleet fleettwo = captain2.CaptainAI.GetFleet();
            foreach (var ship in fleettwo.GetFleet())
            {
                foreach (var coordinate in ship.GetCoordinates())
                {
                    captain2.ShipPlacements[ship.Model, coordinate.X, coordinate.Y]++;
                }
            }
            // While the user has not requested that we stop ...
            int rounds = 0;
            while (!StopCompetition)
            {
                // Run and keep track of rounds during this match
                rounds++;

                // Captain one goes first
                Coordinate attackonecoord = captain1.CaptainAI.MakeAttack(); // Ask captain one for his move
                int attackone = fleettwo.Attacked(attackonecoord); // Determine the result of that move
                captain1.CaptainAI.ResultOfAttack(fleettwo.GetLastAttackValue()); // Inform captain one of the result
                captain2.CaptainAI.OpponentAttack(attackonecoord); // Inform captain two of the result
                captain1.AllAttacks[attackonecoord.X*10 + attackonecoord.Y]++;

                // Did captain one win?
                if (attackone == Constants.Defeated)
                {
                    // Give captain one a point
                    captain1.RecordResultOfGame(true, captain2.CaptainName, rounds);
                    captain2.RecordResultOfGame(false, captain1.CaptainName, rounds);

                    // Stop the match
                    return true;
                }

                // Captain two goes second
                Coordinate attacktwocoord = captain2.CaptainAI.MakeAttack(); // Ask captain two for her move
                int attacktwo = fleetone.Attacked(attacktwocoord); // Determine the result of that move
                captain2.CaptainAI.ResultOfAttack(fleetone.GetLastAttackValue()); // Inform captain two of the result
                captain1.CaptainAI.OpponentAttack(attacktwocoord); // Inform captain one of the result
                captain2.AllAttacks[attacktwocoord.X*10 + attacktwocoord.Y]++;

                // Did captain two win?
                if (attacktwo == Constants.Defeated)
                {
                    // Give captain two a point
                    captain1.RecordResultOfGame(false, captain2.CaptainName, rounds);
                    captain2.RecordResultOfGame(true, captain1.CaptainName, rounds);

                    // Stop the match
                    return false;
                }

                captain1.RecordShot(attackone.IsHit(), captain2.CaptainName);
                captain2.RecordShot(attacktwo.IsHit(), captain1.CaptainName);
            }

            return false;//should be unreachable
        }

        private void OnDoubleClick()
        {
            var window = new CaptainDebugWindow
            {
                DataContext = new CaptainDebugViewModel(SelectedCaptain.GetNewCaptain()) 
            };
            window.Show();
        }

        public List<string> Opponents
        {
            get { return _opponents; }
            set { Set(ref _opponents, value); }
        }
        private List<string> _opponents;

        public List<int> NumberOfMatchesOptions
        {
            get { return _numberOfMatchesOptions; }
            set { Set(ref _numberOfMatchesOptions, value); }
        }
        private List<int> _numberOfMatchesOptions = new List<int> { 100, 500, 1000, 5000, 10000, 50000, 250000, 1000000 };

        public int SelectedNumberOfMatches
        {
            get { return _selectedNumberOfMatches; }
            set { Set(ref _selectedNumberOfMatches, value); }
        }
        private int _selectedNumberOfMatches;

        public int MatchProgress 
        { 
            get{ return _matchProgress; } 
            set{ Set(ref _matchProgress, value); } 
        } 
        private int _matchProgress;

        public int CurrentMatchProgress 
        { 
            get{ return _currentMatchProgress; } 
            set{ Set(ref _currentMatchProgress, value); } 
        } 
        private int _currentMatchProgress;

        public List<Captain> Captains
        {
            get { return _captains; }
            set { Set(nameof(Captains), ref _captains, value); }
        }
        private List<Captain> _captains;

        public Captain SelectedCaptain
        {
            get { return _selectedCaptain; }
            set { Set(ref _selectedCaptain, value); }
        }
        private Captain _selectedCaptain;

        public Captain SelectedOpponentCaptain
        {
            get { return _selectedOpponentCaptain; }
            set
            {
                if (Set(ref _selectedOpponentCaptain, value))
                {
                    var opponent = _selectedOpponentCaptain.CaptainName;
                    if (SelectedCaptain.HasPlayed(opponent))
                    {
                        OpponentStatistics =
                            SelectedCaptain.CaptainStatistics[opponent];
                    }
                }
            }
        }
        private Captain _selectedOpponentCaptain;

        public CaptainStatistics OpponentStatistics
        {
            get { return _opponentStatistics; }
            set { Set(ref _opponentStatistics, value); }
        }
        private CaptainStatistics _opponentStatistics;
        public bool StopCompetition { get; set; }
        public ICommand DoubleClickCommand { get; set; }
        public ICommand RunCompetitionCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand StopCommand { get; set; }
    }
}
