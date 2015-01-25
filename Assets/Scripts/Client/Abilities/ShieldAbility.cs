﻿public class ShieldAbility : BaseAbility
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
        get { return 15; }
    }

    public override float AttackSpeed
    {
        get { return 0f; }
    }
}
