/**
 * <p>A fleet of Battleship pieces placed on a standard 10 by 10 board. There
 * are five ships in a standard Battleship fleet: the patrol boat, the
 * destroyer, the submarine, the battleship and the aircraft carrier. The class
 * helps you to place the pieces on the board in a valid configuration where all
 * of the ships are within the bounds and not overlapping. It also helps you to
 * check if a particular attack has hit a ship and if so, how that attack
 * affects the fleet (was a ship sunk, which ship was it, etc).</p>
 *
 * <p>You should NOT need to do anything to this class in order to participate
 * in the competition and modifying it is discouraged and may make your entry
 * malfunction.</p>
 *
 * @author Seth Dutter - dutters@uwstout.edu
 * @author Seth Berrier - berriers@uwstout.edu
 *
 * @version SPRING.2013
 */

using System.Linq;

namespace Battleship.Core
{
    public class Fleet
    {
        //A fleet is ready once all five ships have been validly placed on the board.
        public bool IsReady { get; protected set; }
        private readonly Ship[] _fleet;
        protected int attackValue ;

        public Fleet()
        {
            IsReady = false;
            attackValue = Constants.Miss;
            _fleet = new Ship[5];
        }

        public Ship[] GetFleet()
        {
            return _fleet;
        }

        protected bool IsValid(int index)
        {
            // Make sure this is a valid ship
            if (!_fleet[index].IsValid())
                return false;

            // Compare it with every other ship in the fleet
            if (_fleet.Any(s => s != null && !s.Equals(_fleet[index]) && _fleet[index].IntersectsShip(s)))
                return false;

            // If every other ship in the fleet is not null then the fleet is ready!
            IsReady = true;
            foreach (var s in _fleet)
            {
                if (s == null)
                    IsReady = false;
            }

            // The placement of this ship is valid
            return true;
        }

        public bool PlaceShip(Coordinate location, int direction, int model)
        {
            if (model >= 6 || model <= -1 || _fleet[model] != null) return false;
            _fleet[model] = new Ship(location, direction, model);
            if (IsValid(model))
            {
                //CaptainDebugger.addShip(location, direction, model, true);
                return true;
            }
            _fleet[model] = null;
            return false;
        }
        
        public bool PlaceShip(int x, int y, int direction, int model)
        {
            return PlaceShip(new Coordinate(x, y), direction, model);
        }

        public bool IsShipAt(Coordinate coord)
        {
            return _fleet.Any(s => s.IsOnShip(coord));
        }

        public bool IsFleetReady()
        {
            return IsReady;
        }

        public int Attacked(Coordinate coord)
        {
            if (!IsFleetReady())
            {
                return Constants.Defeated;    // Forfeit if your fleet isn't complete
            }
            foreach (var s in _fleet)
            {
                attackValue = s.Attacked(coord);
                switch (attackValue - attackValue % 10)
                {
                    case Constants.HitModifier:
                        return attackValue;
                    case Constants.SunkModifier:
                        return _fleet.Any(t => !t.IsSunk()) ? attackValue : Constants.Defeated;
                }
            }
            return Constants.Miss;
        }
    }
}
