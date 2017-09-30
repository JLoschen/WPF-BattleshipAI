/**
 * <p>A set of constants used throughout the Battleship program. These are used
 * to place ship, specify ship types and lengths and determine the results of an
 * attack. Some types of constants can be combined into larger values that
 * encode more information in various decimal digits.</p>
 *
 * <p>An attack code is a two digit number that encodes the type of the ship and
 * the result of an attack: <ul> <li>The 1's digit encodes the type of ship
 * (0=patrol, 1=destroyer, 2=submarine, 3=battleship, 4=carrier)</li> <li>The
 * 10's digit encodes the results of the attack (0=miss, 1=hit, 2=ship sunk)
 * <li>There are two special 1's digits for misses (6) and a full defeat
 * (7)</li> </ul></p>
 *
 * <p>You should not need to change anything in this interface to compete in the
 * competition and modifying it may cause your entry to malfunction.</p>
 *
 * @see Ship Ship
 * @see Fleet Fleet
 * @see Battleship Battleship
 *
 * @author Seth Dutter - dutters@uwstout.edu
 * @author Seth Berrier - berriers@uwstout.edu
 *
 * @version SPRING.2013
 */
namespace Battleship.Core
{
    public static class Constants
    {
        public const int Horizontal = 0;//Orient a ship horizontally on the board
        
        public const int Vertical = 1;//Orient a ship vertically on the board
        
        public const int Won = 1;//Final result of the round is a win
        
        public const int Lost = 0;//Final result of the round is a loss
        
        public const int Miss = 106;//Attack code for a miss
        
        public const int Defeated = 107;//Attack code for defeating your enemy (sinking their final ship)

        public const int HitModifier = 10;//Add this to a type code to get a HIT attack code

        public const int SunkModifier = 20;//Add this to a type code to get a SUNK attack code

        //Ship Type Codes
        public const int PatrolBoat = 0;
        public const int Destroyer = 1;
        public const int Submarine = 2;
        public const int Battleship = 3;
        public const int AircraftCarrier = 4;

        //Ship Lengths
        public const int PatrolBoatLength = 2;
        public const int DestroyerLength = 3;
        public const int SubmarineLength = 3;
        public const int BattleshipLength = 4;
        public const int AircraftCarrierLength = 5;

        //Hit Ship Codes
        public const int HitPatrolBoat = HitModifier + PatrolBoat;
        public const int HitDestroyer = HitModifier + Destroyer;
        public const int HitSubmarine = HitModifier + Submarine;
        public const int HitBattleship = HitModifier + Battleship;
        public const int HitAircraftCarrier = HitModifier + AircraftCarrier;
        
        //Sunk Ship Codes
        public const int SunkPatrolBoat = SunkModifier + PatrolBoat;
        public const int SunkDestroyer = SunkModifier + Destroyer;
        public const int SunkSubmarine = SunkModifier + Submarine;
        public const int SunkBattleship = SunkModifier + Battleship;
        public const int SunkAircraftCarrier = SunkModifier + AircraftCarrier;
    }
}
