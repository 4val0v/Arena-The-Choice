public class SwordAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.Sword; }
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
        get
        {
            var par = Player.BaseData.BaseAttackSpeed;

            foreach (var item in Player.EquippedItems)
            {
                par += ItemsProvider.GetItem(item).AttackSpeed;
            }

            //foreach (var ability in Player.Abilities)
            //{
            //    if (ability == this)
            //        continue;

            //    par += ability.AttackSpeed;
            //}

            return par * AttackSpeedK;
        }
    }

    private const float AttackSpeedK = -0.5f;

    private const int AttacksCount = 3;

    private int _remainAttackCount = AttacksCount;

    public override void UpdateOnAttack()
    {
        _remainAttackCount--;

        if (_remainAttackCount == 0)
        {
           RemoveFromAbilities();
            Logger.Log("remove sword!");
        }
    }
}
