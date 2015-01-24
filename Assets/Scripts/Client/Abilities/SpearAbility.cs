using UnityEngine;
using System.Collections;

public class SpearAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.Spear; }
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
        get { return 240 - Player.BaseData.BaseAttackSpeed; }
    }

    private const int AttacksCount = 4;

    private int _remainAttackCount = AttacksCount;

    public override void UpdateOnAttack()
    {
        _remainAttackCount--;

        if (_remainAttackCount == 0)
        {
            Player.Abilities.Remove(this);
        }
    }
}
