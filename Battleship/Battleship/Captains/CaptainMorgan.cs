using System;
using System.Collections.Generic;
using Battleship.Core;

namespace Battleship.Captains
{
    public class CaptainMorgan:ICaptain
    {
        private int _turnNumber, lastAttackX, lastAttackY, numPossible;
        private readonly int[] shipLengths = { 2, 3, 3, 4, 5 };
        private int[,] targeting;
        private double[,] values;
        private readonly Random _generator;
        private const int Unattacked = 9999;
        private const int Miss = 9998;
        private const int Hit = 9997;
        private const int Unknown = 3;
        private int numGames;
        public static int sinceReset;
        private TrackCoords[,] Board;
        private string _opponent;
        readonly int[] remainingShips = new int[5];
        shipCoord[] sunkShips;
        //int[][] clusterBoard;
        private List<shipCoord> hunterSeeker, allCoords, attackVector;

        public CaptainMorgan()
        {
            _generator = new Random();
        }

        public string GetName()
        {
            return "CaptainMorgan";
        }

        public void Initialize(int numMatches, int numCaptains, string opponent)
        {
            if (numGames % 3 == 0 && numGames > 1)
            {//modifier
                foreach (shipCoord t in hunterSeeker)
                {
                    t.updateHeat();
                }
            }
            if (_opponent == opponent)
            {
                numGames++;
                sinceReset++;
            }
            else//new opponent
            {
                Board = new TrackCoords[10,10];
                targeting = new int[10,10];
                sinceReset = 0;
                hunterSeeker = new List<shipCoord>();
                sunkShips = new shipCoord[5];
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        Board[x,y] = new TrackCoords(x, y);
                        targeting[x,y] = 0;
                        //theirAttacks[x][y] = 0;
                    }
                }
                GenerateHunterSeeker();
            }
            attackVector = new List<shipCoord>();
            GeneratePossiblePlacements();
            values = new double[10,10];
            _turnNumber = 0;
        }

        public void GeneratePossiblePlacements()
        {
            allCoords = new List<shipCoord>();
            for (int ship = 0; ship < 0; ship++)
                remainingShips[ship] = 0;

            int index = 0;
            for (int ship = 4; ship > -1; ship--)
            {
                for (int y = 0; y < 10 - shipLengths[ship] + 1; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        allCoords.Add(new shipCoord(x, y, 1, ship, index));
                        index++;
                        allCoords.Add(new shipCoord(y, x, 0, ship, index));
                        index++;
                        remainingShips[ship] += 2;
                    }
                }
            }
        }

        private void GenerateHunterSeeker()
        {
            int index = 0;
            for (int ship = 0; ship < 5; ship++)
            {
                for (int y = 0; y < 10 - shipLengths[ship] + 1; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        hunterSeeker.Add(new shipCoord(x, y, 1, ship, index));
                        index++;
                        hunterSeeker.Add(new shipCoord(y, x, 0, ship, index));
                        index++;
                    }
                }
            }
        }

        public Fleet GetFleet()
        {
            return GetRandomFleet();
        }

        private Fleet GetRandomFleet()
        {
            var fleet = new Fleet();

            while (!fleet.PlaceShip(_generator.Next(10), _generator.Next(10), _generator.Next(2), Constants.PatrolBoat))
            {
            }
            while (!fleet.PlaceShip(_generator.Next(10), _generator.Next(10), _generator.Next(2), Constants.Destroyer))
            {
            }
            while (!fleet.PlaceShip(_generator.Next(10), _generator.Next(10), _generator.Next(2), Constants.Submarine))
            {
            }
            while (!fleet.PlaceShip(_generator.Next(10), _generator.Next(10), _generator.Next(2), Constants.Battleship))
            {
            }
            while (!fleet.PlaceShip(_generator.Next(10), _generator.Next(10), _generator.Next(2), Constants.AircraftCarrier))
            {
            }

            return fleet;
        }

        public Coordinate MakeAttack()
        {
            _turnNumber++;
            
            int bestX = _generator.Next(10);
            int bestY = _generator.Next(10);
            while (Board[bestX,bestY].Status != Unattacked)
            {
                bestX = _generator.Next(10);
                bestY = _generator.Next(10);
            }
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (Board[x, y].Status != Unattacked || !(values[x, y] >= values[bestX, bestY])) continue;
                    bestX = x;
                    bestY = y;
                }
            }

            lastAttackX = bestX;
            lastAttackY = bestY;
            Board[lastAttackX, lastAttackY].Status = Miss;
            Board[lastAttackX, lastAttackY].Attacked++;//record shot for heat tracking
            targeting[lastAttackX,lastAttackY]++;
            return new Coordinate(lastAttackX, lastAttackY);
        }

        private class TrackCoords
        {
            public int X, Y, Attacked;
            public int Status;
            public TrackCoords(int x, int y)
            {
                X = x;
                Y = y;
                Status = Unattacked;
                Attacked = 0;
            }
        }

        public void ResultOfAttack(int result)
        {
            int hitShip = result % 10;
            if (result == 106)
            {
                removeMiss(lastAttackX, lastAttackY);
                
                //for (PreviousFleet fleet : pastGames)
                //{
                //    if (fleet.board[lastAttackX][lastAttackY])
                //    {
                //        fleet.isPossible = false;
                //        numPossible--;
                //    }
                //}
            }
            if (result / 10 == 1)
            {// shot was a hit
                Board[lastAttackX,lastAttackY].Status = Hit;
                bool inattackVector = false;//whether or not the hit ship is in list
                for (int i = 0; i < attackVector.Count; i++)
                {//go through list of hit ships
                    if (attackVector[i].ship == hitShip)
                    {
                        inattackVector = true;
                    }
                }
                if (!inattackVector)
                {
                    attackVector.Add(new shipCoord(lastAttackX, lastAttackY, Unknown, hitShip, attackVector.Count));
                }
                removeHit(lastAttackX, lastAttackY, hitShip);
            }
            else if (result / 10 == 2)
            {//sunk ship
                Board[lastAttackX,lastAttackY].Status = Hit;
                for (int i = 0; i < attackVector.Count; i++)
                {//go through list of hit ships
                    if (attackVector[i].ship == hitShip)
                    {//records the location of the ship for heat tracking
                        attackVector.RemoveAt(i);//remove sunk ship
                    }
                }
                removeHit(lastAttackX, lastAttackY, hitShip);
                for (int i = 0; i < allCoords.Count; i++)
                {
                    if (allCoords[i].ship == hitShip)
                    {
                        hunterSeeker[allCoords[i].index].used++;
                        shipCoord s = allCoords[i];
                        sunkShips[s.ship] = s;
                        //addToTouchingHit(s);
                        //allCoords.RemoveAt(i);
                        //i--;
                        allCoords.Remove(s);
                    }
                }
            }
            SetValues();
        }

        void removeMiss(int x, int y)
        {//removes the possible opponent placements that overlap the shot(for each ship).
            int remX, remY, length, orient, ship;
            for (int i = 0; i < allCoords.Count; i++)
            {
                ship = allCoords[i].ship;
                remX = allCoords[i].x;
                remY = allCoords[i].y;
                length = shipLengths[allCoords[i].ship];
                orient = allCoords[i].orientation;
                if (x == remX && y <= remY + length - 1 && y >= remY && orient == Constants.Vertical)
                {//vertical removal
                    removeHeat(remX, remY, Constants.Vertical, ship, i);
                    allCoords.RemoveAt(i);
                    i--;
                    remainingShips[ship]--;
                }
                else if (y == remY && x <= remX + length - 1 && x >= remX && orient == Constants.Horizontal)
                {//horizontal removal
                    removeHeat(remX, remY, Constants.Horizontal, ship, i);
                    allCoords.RemoveAt(i);
                    i--;
                    remainingShips[ship]--;
                }//else
            }//for loop
        }

        void removeHeat(int x, int y, int orient, int ship, int i)
        {
            int length = shipLengths[ship];
            switch (orient)
            {
                case Constants.Vertical:
                    for (int z = y; z < y + length; z++)
                    {
                        values[x,z] -= hunterSeeker[allCoords[i].index].heat;
                    }
                    break;
                case Constants.Horizontal:
                    for (int z = x; z < x + length; z++)
                    {
                        values[z,y] -= hunterSeeker[allCoords[i].index].heat;
                    }
                    break;
            }
        }

        private void SetValues()
        {//sets how many ways the remaining ships can be on each tile
            for (int y = 0; y < 10; y++)
            {//resets board's values to 0
                for (int x = 0; x < 10; x++)
                {
                    values[x,y] = 0;
                }
            }
            int remX, remY, length, orient;
            float clusterChance = 1.0f;
            for (int i = 0; i < allCoords.Count; i++)
            {
                remX = allCoords[i].x;
                remY = allCoords[i].y;
                length = shipLengths[allCoords[i].ship];
                orient = allCoords[i].orientation;

                //Not doing whatever this cluster thing is
                //if (length == 2)
                //{
                //    int total = clusterBoard[remX][remY];
                //    if (orient == Constants.Vertical)
                //    {
                //        total += clusterBoard[remX][remY + 1];
                //    }
                //    else
                //    {
                //        total += clusterBoard[remX + 1][remY];
                //    }
                //    clusterChance += 0.05f * total;
                //}
                switch (orient)
                {
                    case Constants.Vertical:
                        for (int z = remY; z < remY + length; z++)
                        {
                            values[remX,z] += (hunterSeeker[allCoords[i].index].heat / (float)remainingShips[allCoords[i].ship]) * clusterChance;
                        }
                        break;
                    case Constants.Horizontal:
                        for (int z = remX; z < remX + length; z++)
                        {
                            values[z,remY] += (hunterSeeker[allCoords[i].index].heat / (float)remainingShips[allCoords[i].ship]) * clusterChance;
                        }
                        break;
                }
            }
        }

        public class shipCoord
        {//Defines the coords generated by generatePossiblePlacements
            public int x, y, index, orientation, ship;
            public double used;
            public double heat = 0.2;
            public shipCoord(int x, int y, int orientation, int ship, int index)
            {//allCoords constructor
                this.x = x;
                this.y = y;
                this.index = index;
                this.orientation = orientation;
                this.ship = ship;
                used = 0;//how often this ship coordinate was used
            }
            public void updateHeat()
            {
                heat = used / (double)sinceReset + 0.00000001;//updates heat for that placement
            }
        }

        void removeHit(int x, int y, int hitShip)
        {
            int remX, remY, length, orient, ship;
            for (int i = 0; i < allCoords.Count; i++)
            {
                ship = allCoords[i].ship;
                remX = allCoords[i].x;
                remY = allCoords[i].y;
                length = shipLengths[ship];
                orient = allCoords[i].orientation;
                if (x != remX && y != remY && hitShip == ship)
                {//vertical removal
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
                else if (y == remY && hitShip == ship && orient == Constants.Vertical && x != remX)
                {
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
                else if (x == remX && hitShip == ship && orient == Constants.Horizontal && y != remY)
                {
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
                else if (x == remX && ship == hitShip && remY > y)
                {//same col above
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
                else if (y == remY && ship == hitShip && x < remX)
                {
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
                else if (x == remX && (remY + length - 1) < y && ship == hitShip && orient == Constants.Vertical)
                {//vertical removal
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
                else if (y == remY && (remX + length - 1) < x && ship == hitShip && orient == Constants.Horizontal)
                {
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }// above removed same ship that couldn't be on hit square
                if (x == remX && y <= remY + length - 1 && y >= remY && orient == Constants.Vertical && ship != hitShip)
                {//vertical removal
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
                if (y == remY && x <= remX + length - 1 && x >= remX && orient == Constants.Horizontal && ship != hitShip)
                {//horizontal removal
                    allCoords.RemoveAt(i);
                    remainingShips[ship]--;
                    i--;
                }
            }
        }

        public void OpponentAttack(Coordinate coord)
        {
            //do nothing    
        }

        public void ResultOfGame(int result)
        {
            throw new NotImplementedException();
        }
    }
}
