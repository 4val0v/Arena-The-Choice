using System;
using System.Collections.Generic;
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
                Name = "RndName"
            };

            EnemyData = new PlayerData();

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
            OnNameUpdated(PlayerData.Name);
        });
    }

    public void SetClass(CharacterClass classId)
    {
        CoroutineExecuter.Execute(() =>
        {
            PlayerData.Class = classId;
            OnClassUpdated(PlayerData.Class);
        });
    }

    public void EquipItem(int itemId)
    {
        throw new NotImplementedException();
    }

    public void SendAttack(int weaponId, int dmg)
    {
        throw new NotImplementedException();
    }

    public void UseAbility(int abilityId)
    {
        throw new NotImplementedException();
    }

    #endregion

    private bool IsLocal { get { return GameMode == GameMode.PvE; } }

    private void ChangeStatus(NetStatus newStatus)
    {
        Status = newStatus;

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
            PlayerData.EquippedItems = new List<int>();

            EnemyData = new PlayerData
            {
                Id = -1,
                Class = CharacterClass.Man,
                EquippedItems = new List<int>(),
                Name = "Bot",
            };

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
        base.OnJoinedLobby();

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonJoinRoomFailed(codeAndMsg);

        PhotonNetwork.CreateRoom(null, new RoomOptions { maxPlayers = 2, isOpen = true, isVisible = true },
            TypedLobby.Default);
    }

    /// <summary>
    /// When we in battle
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.Instantiate("NetPlayer", Vector3.zero, Quaternion.identity, 0);

        PhotonNetwork.player.SetName(PlayerData.Name);
        PhotonNetwork.player.SetClass(PlayerData.Class);
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

    public void RaiseNameUpdated()
    {
        OnNameUpdated(PlayerData.Name);
    }

    public void RaiseEnemyNameUpdated()
    {
        OnNameUpdated(EnemyData.Name);
    }

    public void RaiseClassUpdated()
    {
        OnClassUpdated(PlayerData.Class);
    }

    public void RaiseEnemyClassUpdated()
    {
        OnEnemyClassUpdated(EnemyData.Class);
    }
}
