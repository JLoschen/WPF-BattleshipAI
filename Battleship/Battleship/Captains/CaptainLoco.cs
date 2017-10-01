using System;
using Battleship.Core;

namespace Battleship.Captains
{
    public class CaptainLoco:ICaptain
    {
        protected Random generator;
        protected Fleet myFleet;
        private bool[,] attacked;
        public string GetName()
        {
            return "Captain Loco";
        }

        public void Initialize(int numMatches, int numCaptains, string opponent)
        {
            generator = new Random();
            myFleet = new Fleet();

            while (!myFleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.PatrolBoat))
            {
            }
            while (!myFleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.Destroyer))
            {
            }
            while (!myFleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.Submarine))
            {
            }
            while (!myFleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.Battleship))
            {
            }
            while (!myFleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.AircraftCarrier))
            {
            }

            attacked = new bool[10,10];
        }


        public Fleet GetFleet()
        {
            return myFleet;
        }

        public Coordinate MakeAttack()
        {
            var coord = new Coordinate(generator.Next(10), generator.Next(10));
            while (attacked[coord.X, coord.Y])
            {
                coord = new Coordinate(generator.Next(10), generator.Next(10));
            }
            attacked[coord.X, coord.Y] = true;
            return coord;
        }

        public void ResultOfAttack(int result)
        {
            
        }

        public void OpponentAttack(Coordinate coord)
        {
            
        }

        public void ResultOfGame(int result)
        {
            
        }
    }
}
