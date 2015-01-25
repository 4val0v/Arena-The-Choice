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

    private const int DmgWithAttack = 20;

    public override void UpdateOnAttack()
    {
        NetPlayer.My.AdjHp(-DmgWithAttack);
    }
}
