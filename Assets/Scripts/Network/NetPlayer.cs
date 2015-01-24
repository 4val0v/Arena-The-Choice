﻿using System;
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

    public static NetPlayer My
    {
        get
        {
            foreach (var netPlayer in _players)
            {
                if (netPlayer.photonView.isMine)
                {
                    return netPlayer;
                }
            }

            return null;
        }
    }

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
        int[] items = null;

        switch (step)
        {
            case EquipStep.FirstHand:
                items = new[] { 1, 2, 3, 4 };
                break;
            case EquipStep.SecondHand:
                items = new[] { 5, 6, 7, 8 };
                break;
            case EquipStep.Armor:
                items = new int[] { 9, 10, 11, 12 };
                break;
            default:
                throw new ArgumentOutOfRangeException("step");
        }

        photonView.RPC("StepItemsReceived", PhotonTargets.All, (int)step, items);
    }

    public void SendEquipItem(int itemId)
    {
        photonView.RPC("ItemEquippedReceived", PhotonTargets.All, itemId);
    }

    private void SendGameStart()
    {
        photonView.RPC("GameStartReceived", PhotonTargets.AllViaServer);
    }

    #region handlers

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

        Client.PlayerData.EquippedItems.Clear();
        Client.EnemyData.EquippedItems.Clear();

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

    [RPC]
    public void ItemEquippedReceived(int itemId)
    {
        if (photonView.isMine)
        {
            Client.PlayerData.EquippedItems.Add(itemId);
            Client.RaiseItemEquipped(Client.PlayerData.Id, itemId);
        }
        else
        {
            Client.EnemyData.EquippedItems.Add(itemId);
            Client.RaiseItemEquipped(Client.EnemyData.Id, itemId);
        }

        if (PhotonNetwork.isMasterClient)
        {
            if (Client.Status == NetStatus.ConnectedToBattle)
            {
                if (Client.PlayerData.EquippedItems.Count == 1 && Client.EnemyData.EquippedItems.Count == 1)
                {
                    SetStepItems(EquipStep.SecondHand);
                }
                else if (Client.PlayerData.EquippedItems.Count == 2 && Client.EnemyData.EquippedItems.Count == 2)
                {
                    SetStepItems(EquipStep.Armor);
                }
                else if (Client.PlayerData.EquippedItems.Count == Client.EnemyData.EquippedItems.Count &&
                   Client.PlayerData.EquippedItems.Count == 3)
                {
                    My.SendGameStart();
                }
            }
        }
    }

    [RPC]
    public void GameStartReceived()
    {
        Client.RaiseGameStarted();
    }
    #endregion
}
