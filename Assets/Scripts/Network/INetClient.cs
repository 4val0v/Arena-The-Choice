using System;
using System.Collections.Generic;

public enum NetStatus
{
    /// <summary>
    /// Not in game
    /// </summary>
    Disconnected,
    /// <summary>
    /// Connecting to game
    /// </summary>
    Connecting,
    /// <summary>
    /// In game
    /// </summary>
    Connected,
    /// <summary>
    /// Connecting to battle with bot or player
    /// </summary>
    ConnectingToBattle,
    /// <summary>
    /// In battle
    /// </summary>
    ConnectedToBattle,
    /// <summary>
    /// When disconnecting from game
    /// </summary>
    Disconnecting,
}

public enum NetError
{

}

public enum GameMode
{
    /// <summary>
    /// Player vs Bot
    /// </summary>
    PvE,

    /// <summary>
    /// Player vs Player
    /// </summary>
    PvP,
}

public enum CharacterClass
{
    None = -1,
    Man,
    Woman,
    Orc,
}

public enum CharacterStat
{
    Hp,
    Dmg,
    Def,
    AS,
    //точность
}

public enum EquipStep
{
    FirstHand,
    SecondHand,
    Armor,
}

//
//client's
//
public interface INetClient
{
    /// <summary>
    /// Raise after net status changed
    /// </summary>
    event Action<NetStatus> OnStatusChanged;

    /// <summary>
    /// Raise after name updated
    /// First: new name
    /// </summary>
    event Action<string> OnNameUpdated;

    event Action<string> OnEnemyNameUpdated;

    /// <summary>
    /// Raise after class updated
    /// First: new class
    /// </summary>
    event Action<CharacterClass> OnClassUpdated;

    event Action<CharacterClass> OnEnemyClassUpdated;

    /// <summary>
    /// Raise after random player selection finished
    /// First: playerId, who will be first select item
    /// </summary>
    event Action<int> OnFirstPlayerReceived;

    /// <summary>
    /// Raise when player received items for the equip step
    /// </summary>
    event Action<EquipStep, IEnumerable<int>> OnStepItemsReceived;

    /// <summary>
    /// Raise when the player equipped some item
    /// First: playerId
    /// Second: itemId
    /// </summary>
    event Action<int, int> OnItemEquipped;

    /// <summary>
    /// Raise after game started!
    /// </summary>
    event Action OnGameStarted;

    /// <summary>
    /// Raise when game is finished.
    /// First: winnerId
    /// </summary>
    event Action<int> OnGameFinished;

    event Action<int, int> OnDmgReceived;
    event Action<int, int> OnEnemyDmgReceived;

    event Action<int, int> OnAbilityUsed;
    
    PlayerData PlayerData { get; }

    PlayerData EnemyData { get; }

    /// <summary>
    /// Current net status
    /// </summary>
    NetStatus Status { get; }

    /// <summary>
    /// Current game mode
    /// </summary>
    GameMode GameMode { get; }

    void Connect();

    void Disconnect();

    void CreateOrJoinToBattle(GameMode gameMode);

    void UpdateName(string name);

    void SetClass(CharacterClass classId);

    void EquipItem(int itemId);

    void SendAttack(int weaponId, int dmg);

    void UseAbility(int abilityId);
}