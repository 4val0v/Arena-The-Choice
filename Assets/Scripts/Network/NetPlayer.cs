using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public static class NetPlExtension
{
    public const string NameProp = "plName";
    public const string ClassProp = "plClass";
    public const string HpProp = "plHp";
    public const string DefProp = "plDef";

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

    public static float GetDef(this PhotonPlayer player)
    {
        object teamId;
        if (player.customProperties.TryGetValue(DefProp, out teamId))
        {
            return (float)teamId;
        }

        return 0.0f;
    }

    public static void SetDef(this PhotonPlayer player, float def)
    {
        PhotonNetwork.player.SetCustomProperties(new Hashtable() { { DefProp, def } });
    }
    // public static int GetHp
}
public class NetPlayer : Photon.PunBehaviour
{
    public int Id { get { return photonView.ownerId; } }

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

    public static NetPlayer Enemy
    {
        get
        {
            foreach (var netPlayer in _players)
            {
                if (!netPlayer.photonView.isMine)
                {
                    return netPlayer;
                }
            }

            return null;
        }
    }

    void Awake()
    {
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
        photonView.RPC("BattleStartReceived", PhotonTargets.All, My.Id, Enemy.Id);
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

    private void SendGameFinished(int winnerId)
    {
        photonView.RPC("GameFinishedReceived", PhotonTargets.All, winnerId);
    }

    public void SendAttack(int weaponId, int dmg)
    {
        photonView.RPC("AttackReceived", PhotonTargets.AllViaServer, weaponId, dmg);
    }

    public void SendUseAbility(int abilityId)
    {
        photonView.RPC("AbilityUsedReceived", PhotonTargets.All, abilityId);
    }

    public void AdjHp(int hp)
    {
        photonView.RPC("AdjHpReceived", PhotonTargets.All, hp);
    }
    #region handlers

    [RPC]
    public void BattleStartReceived(int playerId, int otherId)
    {
        if (PhotonNetwork.isMasterClient)
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

    [RPC]
    public void GameFinishedReceived(int winnerId)
    {
        Client.RaiseGameFinished(winnerId);
    }

    [RPC]
    public void AttackReceived(int weaponId, int dmg)
    {
        if (photonView.isMine)
        {
            if (dmg > 0)
                dmg -= (int)Client.EnemyData.Def;

            //attack < def
            if (dmg < 0)
                dmg = 0;

            Client.RaiseEnemyDmgReceived(weaponId, dmg);
        }
        else
        {
            if (dmg > 0)
                dmg -= (int)Client.PlayerData.Def;

            //attack < def
            if (dmg < 0)
                dmg = 0;

            Client.RaiseDmgReceived(weaponId, dmg);
        }

        if (PhotonNetwork.isMasterClient)
        {
            if (Client.PlayerData.CurrentHp <= 0.01f)
            {
                SendGameFinished(Client.EnemyData.Id);
            }
            else if (Client.EnemyData.CurrentHp <= 0.01f)
            {
                SendGameFinished(Client.PlayerData.Id);
            }
        }
    }

    [RPC]
    public void AbilityUsedReceived(int abilityId)
    {
        if (photonView.isMine)
        {
            Client.RaiseAbilityUsed(My.Id, abilityId);
        }
        else
        {
            Client.RaiseAbilityUsed(Enemy.Id, abilityId);
        }
    }

    [RPC]
    public void AdjHpReceived(int hp)
    {
        if (photonView.isMine)
        {
            Client.RaiseHpAdjusted(hp);
        }
        else
        {
            Client.RaiseEnemyHpAdjusted(hp);
        }

        if (PhotonNetwork.isMasterClient)
        {
            if (Client.PlayerData.CurrentHp <= 0.01f)
            {
                SendGameFinished(Client.EnemyData.Id);
            }
            else if (Client.EnemyData.CurrentHp <= 0.01f)
            {
                SendGameFinished(Client.PlayerData.Id);
            }
        }
    }
    #endregion
}
