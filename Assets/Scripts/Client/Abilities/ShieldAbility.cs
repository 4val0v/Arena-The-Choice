public class ShieldAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.Shield; }
    }

    public override float Dmg
    {
        get { return 0f; }
    }

    public override float Def
    {
        get { return 20f; }
    }

    public override float AttackSpeed
    {
        get { return 0f; }
    }

    private const int AttacksCount = 5;

    private int _remainAttackCount = AttacksCount;

    public override void UpdateOnDmgReceive()
    {
        _remainAttackCount--;

        if (_remainAttackCount == 0)
        {
          RemoveFromAbilities();
        }
    }
}
