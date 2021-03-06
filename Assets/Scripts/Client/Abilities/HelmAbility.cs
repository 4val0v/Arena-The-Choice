﻿using UnityEngine;
using System.Collections;

public class HelmAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.Helm; }
    }

    public override float Dmg
    {
        get { return 0f; }
    }

    public override float Def
    {
        get { return 0f; }
    }

    public override float AttackSpeed
    {
        get { return 0f; }
    }

    private const int AttacksCount = 5;
    private const int HealthHp = 50;

    private int _remainAttackCount = AttacksCount;

    public override void UpdateOnAttack()
    {
        Logger.Log("Helm add hp:" + HealthHp);

        NetPlayer.My.AdjHp(HealthHp);

        _remainAttackCount--;

        if (_remainAttackCount == 0)
        {
            RemoveFromAbilities();
            return;
        }
    }
}
