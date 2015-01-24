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
            return Player.AttackSpeed * AttackSpeedK;
        }
    }

    private const float AttackSpeedK = -0.5f;

    private const int AttacksCount = 6;

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
