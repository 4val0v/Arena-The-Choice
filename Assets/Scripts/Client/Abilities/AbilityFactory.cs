public enum AbilityType
{
    Spear = 1,
    Mace = 2,
    Sword = 3,
    Axe = 4,
    Dagger,
    Shield,
    Helm,
}

public static class AbilityFactory
{
    public static BaseAbility CreateAbility(AbilityType ability)
    {
        switch (ability)
        {
            case AbilityType.Spear:
                return new SpearAbility();
            case AbilityType.Mace:
                return new MaceAbility();
            case AbilityType.Sword:
                return new SwordAbility();
            case AbilityType.Axe:
                return new AxeAbility();
            case AbilityType.Dagger:
                return new DaggerAbility();
            case AbilityType.Shield:
                return new ShieldAbility();
            case AbilityType.Helm:
                return new HelmAbility();
        }

        return null;
    }
}