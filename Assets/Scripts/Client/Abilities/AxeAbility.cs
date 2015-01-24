using UnityEngine;
using System.Collections;

public class AxeAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.Axe; }
    }

    public override float Dmg
    {
        get
        {
            foreach (var item in Player.EquippedItems)
            {
                var itemData = ItemsProvider.GetItem(item);
                if (itemData.AbilityId == Id)
                {
                    return itemData.Dmg * DmgK;
                }
            }

            return 0f;
        }
    }

    public override float Def
    {
        get { return 0f; }
    }

    public override float AttackSpeed
    {
        get { return 0f; }
    }

    private const int AttacksCount = 6;
    private const float DmgK = 0.25f;

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
