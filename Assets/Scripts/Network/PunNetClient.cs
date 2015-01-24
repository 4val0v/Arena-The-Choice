using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;

public class PunNetClient : Photon.PunBehaviour, INetClient, INetServer
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
    public event Action<CharacterClass> OnClassUpdated = delegate { };
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

    #region INetServer
    public void SetFirstPlayer(int playerId)
    {
        if (IsLocal)
        {
            FirstPlayerReceived(playerId);
        }
        else
        {
            photonView.RPC("FirstPlayerReceived", PhotonTargets.All, playerId);
        }
    }

    public void SetStepItems(EquipStep step, IEnumerable<int> items)
    {
        OnStepItemsReceived(step, items);
    }

    public void StartGame()
    {
        throw new NotImplementedException();
    }

    public void FinishGame(int winnerId)
    {
        throw new NotImplementedException();
    }
    #endregion

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
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);

        if (PhotonNetwork.playerList.Length == 2 && PhotonNetwork.isMasterClient)
        {
            PlayerData.Id = PhotonNetwork.player.ID;
            
            EnemyData.Id = newPlayer.ID;
            EnemyData.Name = newPlayer.name;

            photonView.RPC("BattleStartReceived", PhotonTargets.All);
        }
    }

    #endregion

    [RPC]
    public void BattleStartReceived()
    {
        OnStatusChanged(NetStatus.ConnectedToBattle);
    }

    [RPC]
    public void FirstPlayerReceived(int playerId)
    {
        OnFirstPlayerReceived(playerId);
    }
}
