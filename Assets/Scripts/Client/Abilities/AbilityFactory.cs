public enum AbilityType
{
    Spear = 1,
    Mace = 2,
    Sword = 3,
    Axe = 4,
    Dagger,
    Shield,
    BigShield,
    Helm,
    EnemyShield,
}

public static class AbilityFactory
{
    public static BaseAbility CreateAbility(AbilityType ability, GameManager gm)
    {
        BaseAbility ab = null;

        switch (ability)
        {
            case AbilityType.Spear:
                ab = new SpearAbility();
                break;
            case AbilityType.Mace:
                ab = new MaceAbility();
                break;
            case AbilityType.Sword:
                ab = new SwordAbility();
                break;
            case AbilityType.Axe:
                ab = new AxeAbility();
                break;
            case AbilityType.Dagger:
                ab = new DaggerAbility();
                break;
            case AbilityType.Shield:
                ab = new ShieldAbility();
                break;
            case AbilityType.BigShield:
                ab = new BigShieldAbility();
                break;
            case AbilityType.EnemyShield:
                ab = new EnemyShieldAbility();
                break;
            case AbilityType.Helm:
                ab = new HelmAbility();
                break;
        }

        if (ab != null)
        {
            ab.Gm = gm;
        }

        return ab;
    }
}