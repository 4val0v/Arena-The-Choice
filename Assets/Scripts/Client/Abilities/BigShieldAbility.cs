public class BigShieldAbility : BaseAbility
{
    public override AbilityType Id
    {
        get { return AbilityType.BigShield; }
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
}
