namespace Battleship.Core
{
    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coordinate coord)
        {
            return X == coord.X && Y == coord.Y;
        }

        public bool LessThan(Coordinate coord)
        {
            return X < coord.X && Y < coord.Y;
        }

        public bool GreaterThan(Coordinate coord)
        {
            return X > coord.X && Y > coord.Y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
