﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Battleship.Core;
using Battleship.TestingWindow;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Battleship.Main
{
    public class MainViewModel: ViewModelBase
    {
        public MainViewModel()
        {
            //_opponents = new List<string> {"Black Beard", "Red Beard", "Dread Pirate Roberts"};
            _numberOfMatchesOptions = new List<int> {500, 1000, 5000, 10000, 50000, 250000, 1000000};

            DoubleClickCommand = new RelayCommand(OnDoubleClick);
            RunCompetitionCommand = new RelayCommand(RunCompetition);
            ResetCommand = new RelayCommand(Reset);

            _captains = new List<Captain>();
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(ICaptain))))
            {
                var captain = new Captain
                {
                    CaptainName = mytype.Name,
                    AssemblyQualifiedName = mytype.AssemblyQualifiedName,
                    CaptainStatistics = new Dictionary<string, CaptainStatistics>(),
                    //AllAttacks = new int[10,10]
                    AllAttacks = new int[100],
                    AttackHeat = new float[100],
                    PlacementHeat = new float[100],
                    AllPlacements = new int[100],
                    //ShowShipPlacement = new [] {true, true, true, true, true},
                    //ShowShipPlacement = new ObservableCollection<bool> { true, true, true, true, true },
                    ShipPlacements = new int[5,10,10]
                };
                _captains.Add(captain);
            }
        }

        private void Reset()
        {
            foreach (var captain in Captains)
            {
                captain.Score = 0;
            }
        }

        private void RunCompetition()
        {
            var captainsToRun = Captains.Where(c => c.IsSelected).ToList();

            foreach (var captain in captainsToRun)
            {
                var captainOpponents = captainsToRun.Where(c => c.AssemblyQualifiedName != captain.AssemblyQualifiedName);
                foreach (var captain2 in captainOpponents)
                {
                    BattleCaptains(captain, captain2, captainsToRun.Count);
                }
            }
        }

        private bool BattleCaptains(Captain captain1, Captain captain2, int numCaptains)
        {
            // reset the progress bar
            //currentProgressBar.setValue(0);

            var captainOne = GetCaptain(captain1);
            var captainTwo = GetCaptain(captain2);
            // Get the names of the two captains
            string nameOne = captain1.CaptainName;//BattleshipTableModel.nameNoPackage(captainone.getClass());
            string nameTwo = captain2.CaptainName;//BattleshipTableModel.nameNoPackage(captaintwo.getClass());

            if (!captain1.CaptainStatistics.ContainsKey(nameTwo))//if haven't played before
            {
                captain1.CaptainStatistics.Add(new KeyValuePair<string, CaptainStatistics>(nameTwo, new CaptainStatistics()));
                captain2.CaptainStatistics.Add(new KeyValuePair<string, CaptainStatistics>(nameOne, new CaptainStatistics()));
            }

            // Remember their scores (how many matches they have won).
            int scoreOne = 0, scoreTwo = 0;

            //// Add them to the detailed records if they haven't been put there already
            //if (!detailedRecords.containsKey(nameOne))
            //{
            //    detailedRecords.put(nameOne, new CaptainStatistics(nameOne));
            //}
            //if (!detailedRecords.containsKey(nameTwo))
            //{
            //    detailedRecords.put(nameTwo, new CaptainStatistics(nameTwo));
            //}

            // Skip this battle if either captain is disabled
            //if (!battleModel.isCaptainEnabled(nameOne) || !battleModel.isCaptainEnabled(nameTwo))
            //{
            //    return false;
            //}

            //int[][] startingOneAtk = detailedRecords.get(nameOne).getAttackPattern();
            //int[][] startingOnePlc = detailedRecords.get(nameOne).getShipPlacement();

            //int[][] startingTwoAtk = detailedRecords.get(nameTwo).getAttackPattern();
            //int[][] startingTwoPlc = detailedRecords.get(nameTwo).getShipPlacement();

            var halfNumberOfMatches = SelectedNumberOfMatches / 2;
            for (int i = 0; i < halfNumberOfMatches; i++)
            {
                // Initialize the first captain and his fleet
                captainOne.Initialize(/*2 * halfNumberOfMatches*/ SelectedNumberOfMatches, numCaptains, nameTwo);

                // Record his ship placement choices
                Fleet fleetone = captainOne.GetFleet();
                //for (int x = 0; x < 10; x++)
                //{
                //    for (int y = 0; y < 10; y++)
                //    {
                //        if (fleetone.IsShipAt(new Coordinate(x, y)))
                //        {
                //            captain1.AllPlacements[x * 10 + y]++;
                //        }
                //    }
                //}
                foreach (var ship in fleetone.GetFleet())
                {
                    foreach (var coordinate in ship.GetCoordinates())
                    {
                        captain1.ShipPlacements[ship.Model, coordinate.X, coordinate.Y]++;
                    }
                }

                // Add these ship placement choices to the statistics against this particular opponent
                //detailedRecords.get(nameOne).addNewGame(shipLocs, nameTwo);

                // Initialize the second captain and her fleet
                captainTwo.Initialize(SelectedNumberOfMatches, numCaptains, nameOne);

                // Record her ship placement choices
                Fleet fleettwo = captainTwo.GetFleet();
                //for (int x = 0; x < 10; x++)
                //{
                //    for (int y = 0; y < 10; y++)
                //    {
                //        if (fleettwo.IsShipAt(new Coordinate(x, y)))
                //        {
                //            captain2.AllPlacements[x * 10 + y]++;
                //        }
                //    }
                //}
                foreach (var ship in fleettwo.GetFleet())
                {
                    foreach (var coordinate in ship.GetCoordinates())
                    {
                        captain2.ShipPlacements[ship.Model, coordinate.X, coordinate.Y]++;
                    }
                }

                // Add these ship placement choices to the statistics against this particular opponent
                //detailedRecords.get(nameTwo).addNewGame(shipLocs, nameOne);

                if (i % 2 == 0)
                {
                    // While the user has not requested that we stop ...
                    int rounds = 0;
                    while (/*keepGoing*/true)
                    {
                        // Run and keep track of rounds during this match
                        rounds++;

                        // Captain one goes first
                        Coordinate attackonecoord = captainOne.MakeAttack();    // Ask captain one for his move
                        int attackone = fleettwo.Attacked(attackonecoord);      // Determine the result of that move
                        captainOne.ResultOfAttack(fleettwo.GetLastAttackValue());// Inform captain one of the result
                        captainTwo.OpponentAttack(attackonecoord);              // Inform captain two of the result
                        captain1.AllAttacks[attackonecoord.X *10 + attackonecoord.Y]++;

                        // Did captain one win?
                        if (attackone == Constants.Defeated)
                        {
                            // Give captain one a point
                            scoreOne++;
                            // Record the move
                            //detailedRecords.get(nameOne).addRound(nameTwo, true, attackonecoord);
                            captain1.CaptainStatistics[nameTwo].WinAttacks += rounds;
                            captain1.CaptainStatistics[nameTwo].Wins++;
                            captain2.CaptainStatistics[nameOne].LossAttacks += rounds;
                            captain2.CaptainStatistics[nameOne].Losses++;

                            // Record the results of the match
                            //detailedRecords.get(nameOne).addFinishedGame(nameTwo, true, rounds);
                            //detailedRecords.get(nameTwo).addFinishedGame(nameOne, false, rounds);

                            // Tell them the results of this match
                            captainOne.ResultOfGame(Constants.Won);
                            captainTwo.ResultOfGame(Constants.Lost);

                            // Stop the match
                            break;
                        }

                        // Captain two goes second
                        Coordinate attacktwocoord = captainTwo.MakeAttack();    // Ask captain two for her move
                        int attacktwo = fleetone.Attacked(attacktwocoord);      // Determine the result of that move
                        captainTwo.ResultOfAttack(fleetone.GetLastAttackValue());// Inform captain two of the result
                        captainOne.OpponentAttack(attacktwocoord);              // Inform captain one of the result
                        captain2.AllAttacks[attacktwocoord.X *10 + attacktwocoord.Y]++;

                        // Did captain two win?
                        if (attacktwo == Constants.Defeated)
                        {
                            // Give captain two a point
                            scoreTwo++;
                            // Record the move
                            //detailedRecords.get(nameTwo).addRound(nameOne, true, attacktwocoord);
                            captain2.CaptainStatistics[nameOne].WinAttacks += rounds;
                            captain2.CaptainStatistics[nameOne].Wins++;
                            captain1.CaptainStatistics[nameTwo].LossAttacks += rounds;
                            captain1.CaptainStatistics[nameTwo].Losses++;

                            // Record the results of the match
                            //detailedRecords.get(nameTwo).addFinishedGame(nameOne, true, rounds);
                            //detailedRecords.get(nameOne).addFinishedGame(nameTwo, false, rounds);

                            captainTwo.ResultOfGame(Constants.Won);
                            captainOne.ResultOfGame(Constants.Lost);
                            
                            break;
                        }

                        // Was the result of either attack a hit?
                        bool oneHit = attackone / Constants.HitModifier == 1 || attackone / Constants.HitModifier == 2;
                        bool twoHit = attacktwo / Constants.HitModifier == 1 || attacktwo / Constants.HitModifier == 2;

                        if (oneHit)
                        {
                            captain1.CaptainStatistics[nameTwo].Hits++;
                        }
                        else
                        {
                            captain1.CaptainStatistics[nameTwo].Misses++;
                        }

                        if (twoHit)
                        {
                            captain2.CaptainStatistics[nameOne].Hits++;
                        }
                        else
                        {
                            captain2.CaptainStatistics[nameOne].Misses++;
                        }

                        // Record these two moves in the statistics
                        //detailedRecords.get(nameOne).addRound(nameTwo, oneHit, attackonecoord);
                        //detailedRecords.get(nameTwo).addRound(nameOne, twoHit, attacktwocoord);
                    }
                }
                else
                {
                    // While the user has not requested that we stop ...
                    int rounds = 0;
                    while (/*keepGoing*/true)
                    {
                        // Run and keep track of rounds during this match
                        rounds++;

                        // Captain two goes first
                        Coordinate attacktwocoord = captainTwo.MakeAttack();    // Ask captain two for her move
                        int attacktwo = fleetone.Attacked(attacktwocoord);      // Determine the result of that move
                        captainTwo.ResultOfAttack(fleetone.GetLastAttackValue());// Inform captain two of the result
                        captainOne.OpponentAttack(attacktwocoord);              // Inform captain one of the result

                        // Did captain two win?
                        if (attacktwo == Constants.Defeated)
                        {
                            // Give captain two a point
                            scoreTwo++;
                            // Record the move
                            //detailedRecords.get(nameTwo).addRound(nameOne, true, attacktwocoord);
                            captain2.CaptainStatistics[nameOne].WinAttacks += rounds;
                            captain2.CaptainStatistics[nameOne].Wins++;
                            captain1.CaptainStatistics[nameTwo].LossAttacks += rounds;
                            captain1.CaptainStatistics[nameTwo].Losses++;
                            // Record the results of the match
                            //detailedRecords.get(nameTwo).addFinishedGame(nameOne, true, rounds);
                            //detailedRecords.get(nameOne).addFinishedGame(nameTwo, false, rounds);

                            // Tell them the results of this match
                            captainTwo.ResultOfGame(Constants.Won);
                            captainOne.ResultOfGame(Constants.Lost);

                            // Stop the match
                            break;
                        }

                        // Captain one goes second
                        Coordinate attackonecoord = captainOne.MakeAttack();    // Ask captain one for his move
                        int attackone = fleettwo.Attacked(attackonecoord);      // Determine the result of that move
                        captainOne.ResultOfAttack(fleettwo.GetLastAttackValue()); // Inform captain one of the result
                        captainTwo.OpponentAttack(attackonecoord);              // Inform captain two of the result

                        // Did captain one win?
                        if (attackone == Constants.Defeated)
                        {
                            // Give captain one a point
                            scoreOne++;
                            captain1.CaptainStatistics[nameTwo].WinAttacks += rounds;
                            captain1.CaptainStatistics[nameTwo].Wins++;
                            captain2.CaptainStatistics[nameOne].LossAttacks += rounds;
                            captain2.CaptainStatistics[nameOne].Losses++;
                            // Record the move
                            //detailedRecords.get(nameOne).addRound(nameTwo, true, attackonecoord);

                            // Record the results of the match
                            //detailedRecords.get(nameOne).addFinishedGame(nameTwo, true, rounds);
                            //detailedRecords.get(nameTwo).addFinishedGame(nameOne, false, rounds);

                            // Tell them the results of this match
                            captainOne.ResultOfGame(Constants.Won);
                            captainTwo.ResultOfGame(Constants.Lost);

                            // Stop the match
                            break;
                        }

                        // Was the result of either attack a hit?
                        bool oneHit = (attackone / Constants.HitModifier == 1 || attackone / Constants.HitModifier == 2);
                        bool twoHit = (attacktwo / Constants.HitModifier == 1 || attacktwo / Constants.HitModifier == 2);

                        if (oneHit)
                        {
                            captain1.CaptainStatistics[nameTwo].Hits++;
                        }
                        else
                        {
                            captain1.CaptainStatistics[nameTwo].Misses++;
                        }

                        if (twoHit)
                        {
                            captain2.CaptainStatistics[nameOne].Hits++;
                        }
                        else
                        {
                            captain2.CaptainStatistics[nameOne].Misses++;
                        }

                        // Record these two moves in the statistics
                        //detailedRecords.get(nameOne).addRound(nameTwo, oneHit, attackonecoord);
                        //detailedRecords.get(nameTwo).addRound(nameOne, twoHit, attacktwocoord);
                    }
                }


                // Has the user requested that we stop?
                //if (!keepGoing)
                //{
                //    return false;
                //}

                // update the progress bar
                //currentProgressBar.setValue((100 * (i + 1)) / (2 * halfNumberOfMatches));

                if (i % 500 == 0 && i > 0)
                {
                    //battleModel.addCaptainScore(nameOne, scoreOne);
                    //battleModel.addCaptainScore(nameTwo, scoreTwo);
                    //captain2.Score += scoreTwo;
                    //captain1.Score += scoreOne;
                    captain2.Score = captain2.Score + scoreTwo;
                    captain1.Score = captain1.Score + scoreOne;
                    //table.repaint();
                    scoreOne = 0;
                    scoreTwo = 0;
                }

                //if ((i + 1) % (halfNumberOfMatches * .02) == 0)
                //{
                //    detailedRecords.get(nameOne).addSample(nameTwo, startingOneAtk, startingOnePlc);
                //    detailedRecords.get(nameTwo).addSample(nameOne, startingTwoAtk, startingTwoPlc);
                //}
            }
            captain2.Score = captain2.Score + scoreTwo;
            captain1.Score = captain1.Score + scoreOne;

            captain1.UpdateGui();
            captain2.UpdateGui();

            // Update the score model and repaint the table
            //battleModel.addCaptainScore(nameOne, scoreOne);
            //battleModel.addCaptainScore(nameTwo, scoreTwo);
            //table.repaint();

            // Battle was successful!
            return true;
        }

        private void OnDoubleClick()
        {
            var window = new CaptainDebugWindow {DataContext = new CaptainDebugViewModel(/*new CaptainMorgan()*//*myObject*/GetCaptain(SelectedCaptain)) };
            window.Show();
        }

        private ICaptain GetCaptain(Captain captain)
        {
            var type = Type.GetType(captain.AssemblyQualifiedName);
            return (ICaptain)Activator.CreateInstance(type);
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

        public int SelectedNumberOfMatches 
        { 
            get{ return _selectedNumberOfMatches; } 
            set{ Set(ref _selectedNumberOfMatches, value); } 
        } 
        private int _selectedNumberOfMatches;

        public List<Captain> Captains 
        { 
            get{ return _captains; } 
            set{ Set(nameof(Captains),ref _captains, value); } 
        } 
        private List<Captain> _captains;

        //public bool ShowPatrolPlacement 
        //{ 
        //    get{ return _showPatrolPlacement; } 
        //    set{ Set(ref _showPatrolPlacement, value); } 
        //} 
        //private bool _showPatrolPlacement;

        //public bool ShowDestroyerPlacement 
        //{ 
        //    get{ return _showDestroyerPlacement; } 
        //    set{ Set(ref _showDestroyerPlacement, value); } 
        //} 
        //private bool _showDestroyerPlacement;

        //public bool ShowSubmarinePlacement 
        //{ 
        //    get{ return _showSubmarinePlacement; } 
        //    set{ Set(ref _showSubmarinePlacement, value); } 
        //} 
        //private bool _showSubmarinePlacement;
    
        //public bool ShowBattleshipPlacement 
        //{ 
        //    get{ return _showBattleshipPlacement; } 
        //    set{ Set(ref _showBattleshipPlacement, value); } 
        //} 
        //private bool _showBattleshipPlacement;

        //public bool ShowAircraftCarrierPlacement 
        //{ 
        //    get{ return _showAircraftCarrierPlacement; } 
        //    set{ Set(ref _showAircraftCarrierPlacement, value); } 
        //} 
        //private bool _showAircraftCarrierPlacement;

        public Captain SelectedCaptain 
        { 
            get{ return _selectedCaptain; }
            set
            {
                if (Set(ref _selectedCaptain, value))
                {
                    Console.WriteLine("result!");
                }
            } 
        } 
        private Captain _selectedCaptain;

        public ICommand DoubleClickCommand { get; set; }
        public ICommand RunCompetitionCommand { get; set; }
        public ICommand ResetCommand { get; set; }
    }
}
