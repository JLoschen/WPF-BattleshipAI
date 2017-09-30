/**
 * A representation of a single piece for the game Battleship. This class works
 * in conjunction with {@link Constants} to allow easy examination and
 * comparison of pieces in the game. It is used extensively by {@link Fleet}.
 * You should not need to change anything in this interface to compete in the
 * competition and modifying it may cause your entry to be disqualified!
 *
 * @author Seth Dutter - dutters@uwstout.edu
 * @author Seth Berrier - berriers@uwstout.edu
 *
 * @version SPRING.2013
 */
namespace Battleship.Core
{
    public class Ship
    {
        protected const int Hit = 1;
        public Coordinate Location { get; protected set; }
        public int Length { get; protected set; }
        public int Direction { get; protected set; }
        protected int[] ShipData { get; set; }
        public bool Sunk { get; protected set; }
        public int Model { get; protected set; }

        public Ship(Coordinate location, int direction, int model)
        {
            Location = location;
            Direction = direction;
            Model = model;
            Sunk = false;
            switch (model)
            {
                case Constants.PatrolBoat:
                    Length = Constants.PatrolBoatLength;
                    break;
                case Constants.Destroyer:
                    Length = Constants.DestroyerLength;
                    break;
                case Constants.Submarine:
                    Length = Constants.SubmarineLength;
                    break;
                case Constants.Battleship:
                    Length = Constants.BattleshipLength;
                    break;
                default:
                    Length = Constants.AircraftCarrierLength;
                    break;
            }

            ShipData = new int[Length];
        }

        public bool IsSunk()
        {
            return Sunk;
        }

        public bool IsValid()
        {
            if (Direction == Constants.Horizontal)
            {
                var farEnd = new Coordinate(Location.X + Length - 1, Location.Y);
                return Location.GreaterThan(new Coordinate(-1, -1)) && farEnd.LessThan(new Coordinate(10, 10));
            }
            else
            {
                var farEnd = new Coordinate(Location.X, Location.Y + Length - 1);
                return Location.GreaterThan(new Coordinate(-1, -1)) && farEnd.LessThan(new Coordinate(10, 10));
            }
        }

        public bool IsOnShip(Coordinate coord)
        {
            if (Direction == Constants.Horizontal)
            {
                if (Location.Y != coord.Y) return false;
                for (var i = 0; i < Length; i++)
                {
                    if (Location.X + i == coord.X)
                        return true;
                }
                return false;
            }

            if (Location.X != coord.X) return false;
            for (var i = 0; i < Length; i++)
            {
                if (Location.Y + i == coord.Y)
                    return true;
            }
            return false;
        }

        public bool IntersectsShip(Ship ship)
        {
            if (Direction == Constants.Horizontal)
            {
                for (var i = 0; i < Length; i++)
                {
                    if (ship.IsOnShip(new Coordinate(Location.X + i, Location.Y)))
                        return true;
                }
            }
            else
            {
                for (var i = 0; i < Length; i++)
                {
                    if (ship.IsOnShip(new Coordinate(Location.X, Location.Y + i)))
                        return true;
                }
            }
            return false;
        }

        public int Attacked(Coordinate coord)
        {
            if (Direction == Constants.Horizontal)
            {
                if (Location.Y != coord.Y) return Constants.Miss;
                for (var i = 0; i < Length; i++)
                {
                    if (Location.X + i != coord.X) continue;
                    ShipData[i] = Hit;
                    for (var j = 0; j < Length; j++)
                    {
                        if (ShipData[j] != Hit)
                            return Model + Constants.HitModifier;
                    }
                    Sunk = true;
                    return Model + Constants.SunkModifier;
                }
                return Constants.Miss;
            }
            if (Location.X != coord.X) return Constants.Miss;
            for (var i = 0; i < Length; i++)
            {
                if (Location.Y + i != coord.Y) continue;
                ShipData[i] = Hit;
                for (var j = 0; j < Length; j++)
                {
                    if (ShipData[j] != Hit)
                        return Model + Constants.HitModifier;
                }
                Sunk = true;
                return Model + Constants.SunkModifier;
            }
            return Constants.Miss;
        }
    }
}
