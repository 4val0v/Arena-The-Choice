using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public static class NetPlExtension
{
    public const string NameProp = "plName";
    public const string ClassProp = "plClass";
    public const string HpProp = "plHp";

    public static string GetName(this PhotonPlayer player)
    {
        object teamId;
        if (player.customProperties.TryGetValue(NameProp, out teamId))
        {
            return (string)teamId;
        }

        return string.Empty;
    }

    public static void SetName(this PhotonPlayer player, string name)
    {
        PhotonNetwork.player.SetCustomProperties(new Hashtable() { { NameProp, name } });
    }

    public static CharacterClass GetClass(this PhotonPlayer player)
    {
        object teamId;
        if (player.customProperties.TryGetValue(ClassProp, out teamId))
        {
            return (CharacterClass)(int)teamId;
        }

        return CharacterClass.Man;
    }

    public static void SetClass(this PhotonPlayer player, CharacterClass name)
    {
        PhotonNetwork.player.SetCustomProperties(new Hashtable() { { ClassProp, (int)name } });
    }

    // public static int GetHp
}
public class NetPlayer : Photon.PunBehaviour
{
    public int Id { get { return photonView.ownerId; } }
    private static NetPlayer _master;
    private static NetPlayer _other;

    private Room Room { get { return PhotonNetwork.room; } }
    private PunNetClient Client { get { return (PunNetClient)PunNetClient.Instance; } }

    public bool IsMaster { get { return photonView.owner.isMasterClient; } }

    private readonly static List<NetPlayer> _players = new List<NetPlayer>();

    void Awake()
    {
        if (photonView.owner.isMasterClient)
            _master = this;
        else
            _other = this;

        _players.Add(this);
    }

    void OnDestroy()
    {
        _players.Remove(this);
    }

    void Update()
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        if (!IsMaster)
            return;

        if (Client.Status == NetStatus.ConnectingToBattle && _players.Count == 2)
        {
            SendStartBattle();
        }
    }

    private void SendStartBattle()
    {
        Client.PlayerData.Id = PhotonNetwork.player.ID;

        photonView.RPC("BattleStartReceived", PhotonTargets.All, Id, _other.Id);
    }

    private void SetFirstPlayer()
    {
        var firstPlayerId = PhotonNetwork.playerList[UnityEngine.Random.Range(0, Room.playerCount)].ID;

        photonView.RPC("FirstPlayerReceived", PhotonTargets.All, firstPlayerId);
    }

    private void SetStepItems(EquipStep step)
    {
        int[] items = new[] { 1 };
        photonView.RPC("StepItemsReceived", PhotonTargets.All, (int)step, items);
    }

    [RPC]
    public void BattleStartReceived(int playerId, int otherId)
    {
        if (IsMaster)
        {
            Client.PlayerData.Id = playerId;
            Client.EnemyData.Id = otherId;
        }
        else
        {
            Client.EnemyData.Id = playerId;
            Client.PlayerData.Id = otherId;
        }

        Client.RaiseBattleStarted();

        if (PhotonNetwork.isMasterClient)
        {
            SetFirstPlayer();
            SetStepItems(EquipStep.FirstHand);
        }
    }

    [RPC]
    public void FirstPlayerReceived(int playerId)
    {
        Client.RaiseFirstPlayerStep(playerId);
    }

    [RPC]
    public void StepItemsReceived(int step, int[] items)
    {
        Client.RaiseStepItems((EquipStep)step, items);
    }
}
