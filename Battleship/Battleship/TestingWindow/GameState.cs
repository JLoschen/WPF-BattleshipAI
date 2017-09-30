
namespace Battleship.TestingWindow
{
    public enum GameState
    {
        Waiting, //before game, or after game
        HumanPlayerPlacingPatrol,
        HumanPlayerPlacingSubmarine,
        HumanPlayerPlacingDestroyer,
        HumanPlayerPlacingBattleship,
        HumanPlayerPlacingAircraftCarrier,
        ReadyWaitingToStart,
        HumansTurn,
        AIsTurn
    }
}
