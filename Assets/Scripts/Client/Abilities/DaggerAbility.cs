public class DaggerAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.Dagger; }
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

    private const float DmgWithAttack = 20;
    private const int AttacksCount = 6;

    private int _remainAttackCount = AttacksCount;

    public override void UpdateOnAttack()
    {
        _remainAttackCount--;

        if (_remainAttackCount == 0)
        {
            RemoveFromAbilities();
            return;
        }

        Player.CurrentHp -= DmgWithAttack;
    }
}
