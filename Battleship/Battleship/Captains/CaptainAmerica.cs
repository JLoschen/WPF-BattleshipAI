using System;
using Battleship.Core;

namespace Battleship.Captains
{
    public class CaptainAmerica : ICaptain
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

            attacked = new bool[10, 10];
        }

        private Fleet GetRandomFleet()
        {
            var fleet = new Fleet();

            while (!fleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.PatrolBoat))
            {
            }
            while (!fleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.Destroyer))
            {
            }
            while (!fleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.Submarine))
            {
            }
            while (!fleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.Battleship))
            {
            }
            while (!fleet.PlaceShip(generator.Next(10), generator.Next(10), generator.Next(2), Constants.AircraftCarrier))
            {
            }

            return fleet;
        }

        public Fleet GetFleet()
        {
            return GetRandomFleet();
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
