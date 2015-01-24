using System.Collections.Generic;

//
//server side
//
public interface INetServer
{
    void SetFirstPlayer(int playerId);
    void SetStepItems(EquipStep step, IEnumerable<int> items);
    void StartGame();
    void FinishGame(int winnerId);
}