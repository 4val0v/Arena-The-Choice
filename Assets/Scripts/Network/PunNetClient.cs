using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class PunNetClient : Photon.PunBehaviour, INetClient
{
    private static INetClient _instance;

    public static INetClient Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("PunNetClient").AddComponent<PunNetClient>();
            }

            return _instance;
        }
    }

    #region INetClient
    public event Action<NetStatus> OnStatusChanged = delegate { };
    public event Action<string> OnNameUpdated = delegate { };
    public event Action<string> OnEnemyNameUpdated = delegate { };
    public event Action<CharacterClass> OnClassUpdated = delegate { };
    public event Action<CharacterClass> OnEnemyClassUpdated = delegate { };
    public event Action<int> OnFirstPlayerReceived = delegate { };
    public event Action<EquipStep, IEnumerable<int>> OnStepItemsReceived = delegate { };
    public event Action<int, int> OnItemEquipped = delegate { };
    public event Action OnGameStarted = delegate { };
    public event Action<int> OnGameFinished = delegate { };
    public event Action<int, int> OnDmgReceived = delegate { };
    public event Action<int, int> OnEnemyDmgReceived = delegate { };
    public event Action<int, int> OnAbilityUsed = delegate { };

    public PlayerData PlayerData { get; private set; }
    public PlayerData EnemyData { get; private set; }

    public NetStatus Status { get; private set; }
    public GameMode GameMode { get; private set; }

    public void Connect()
    {
        ChangeStatus(NetStatus.Connecting);

        CoroutineExecuter.Execute(() =>
        {
            PlayerData = new PlayerData
            {
                Class = CharacterClass.None,
            };
            
            EnemyData = new PlayerData
            {
                Class = CharacterClass.None
            };

            ChangeStatus(NetStatus.Connected);
        });
    }

    public void Disconnect()
    {
        ChangeStatus(NetStatus.Disconnecting);

        CoroutineExecuter.Execute(() =>
        {
            PlayerData = EnemyData = null;

            ChangeStatus(NetStatus.Disconnected);
        });
    }

    public void CreateOrJoinToBattle(GameMode gameMode)
    {
        CoroutineExecuter.Execute(() =>
        {
            ChangeStatus(NetStatus.ConnectingToBattle);

            GoToGameMode(gameMode);
        });
    }

    public void UpdateName(string name)
    {
        CoroutineExecuter.Execute(() =>
        {
            PlayerData.Name = name;
            PhotonNetwork.player.SetName(name);
        });
    }

    public void SetClass(CharacterClass classId)
    {
        CoroutineExecuter.Execute(() =>
        {
            PlayerData.Class = classId;
            PhotonNetwork.player.SetClass(classId);
        });
    }

    public void EquipItem(int itemId)
    {
        NetPlayer.My.SendEquipItem(itemId);
    }

    public void SendAttack(int weaponId, int dmg)
    {
        NetPlayer.My.SendAttack(weaponId, dmg);
    }

    public void UseAbility(int abilityId)
    {
        NetPlayer.My.SendUseAbility(abilityId);
    }

    #endregion

    private bool IsLocal { get { return GameMode == GameMode.PvE; } }

    private void ChangeStatus(NetStatus newStatus)
    {
        Status = newStatus;

        if (Status == NetStatus.Disconnected)
        {
            PlayerData = EnemyData = null;
            PhotonNetwork.Disconnect();
        }

        OnStatusChanged(Status);
    }

    /// <summary>
    /// Set Id
    /// </summary>
    /// <param name="gameMode"></param>
    private void GoToGameMode(GameMode gameMode)
    {
        //local game
        if (gameMode == GameMode.PvE)
        {
            PlayerData.Id = 1;
            PlayerData.EquippedItems.Clear();

            EnemyData = new PlayerData
            {
                Id = -1,
                Class = CharacterClass.Man,
                Name = "Bot",
            };
            EnemyData.EquippedItems.Clear();

            GameMode = gameMode;

            ChangeStatus(NetStatus.ConnectedToBattle);

            //who first step?

        }
        //net game
        else if (gameMode == GameMode.PvP)
        {
            ConnectToPhotonRoom();
        }
        else
        {
            throw new ArgumentException();
        }
    }

    private void ConnectToPhotonRoom()
    {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings("1.0");
    }

    #region Photon's callbacks

    public override void OnJoinedLobby()
    {
        Logger.Log("OnJoinedLobby");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Logger.Log("OnPhotonRandomJoinFailed");

        PhotonNetwork.CreateRoom(null, new RoomOptions { maxPlayers = 2, isOpen = true, isVisible = true },
            TypedLobby.Default);
    }

    /// <summary>
    /// When we in battle
    /// </summary>
    public override void OnJoinedRoom()
    {
        Logger.Log("OnJoinedRoom");

        PhotonNetwork.Instantiate("NetPlayer", Vector3.zero, Quaternion.identity, 0);
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
        Hashtable props = playerAndUpdatedProps[1] as Hashtable;

        PlayerData target;

        if (player.isLocal)
        {
            target = PlayerData;
        }
        else
        {
            target = EnemyData;
        }


        if (props.ContainsKey(NetPlExtension.NameProp))
        {
            target.Name = player.GetName();

            if (player.isLocal)
            {
                OnNameUpdated(PlayerData.Name);
            }
            else
            {
                OnEnemyNameUpdated(EnemyData.Name);
            }

        }
        else if (props.ContainsKey(NetPlExtension.ClassProp))
        {
            target.Class = player.GetClass();

            if (player.isLocal)
            {
                OnClassUpdated(PlayerData.Class);
            }
            else
            {
                OnEnemyClassUpdated(EnemyData.Class);
            }
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);

        Disconnect();
    }

    #endregion

    public void RaiseBattleStarted()
    {
        ChangeStatus(NetStatus.ConnectedToBattle);
    }

    public void RaiseFirstPlayerStep(int playerId)
    {
        OnFirstPlayerReceived(playerId);
    }

    public void RaiseStepItems(EquipStep step, IEnumerable<int> items)
    {
        OnStepItemsReceived(step, items);
    }

    public void RaiseItemEquipped(int playerId, int itemId)
    {
        OnItemEquipped(playerId, itemId);
    }

    public void RaiseGameStarted()
    {
        OnGameStarted();
    }

    public void RaiseGameFinished(int winnerId)
    {
        OnGameFinished(winnerId);
    }

    public void RaiseEnemyDmgReceived(int weaponId, int dmg)
    {
        OnEnemyDmgReceived(weaponId, dmg);
    }

    public void RaiseDmgReceived(int weaponId, int dmg)
    {
        OnDmgReceived(weaponId, dmg);
    }

    public void RaiseAbilityUsed(int whoId, int abilityId)
    {
        OnAbilityUsed(whoId, abilityId);
    }
}
