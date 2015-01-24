using System.Collections.Generic;

public static class AbilitiesProvider
{
    public static readonly List<AbilityData> Items;

    static AbilitiesProvider()
    {
        Items = new List<AbilityData>();

        //Spear
        Items.Add(new AbilityData(AbilityType.Spear)
        {
            Description = "3 spear's attacks",
            Cooldown = 20,
        });

        //Mace
        Items.Add(new AbilityData(AbilityType.Mace)
        {
            Description = "+200% Dmg",
            Cooldown = 30,
        });

        //Sword
        Items.Add(new AbilityData(AbilityType.Sword)
        {
            Description = "-20% Attack speed",
            Cooldown = 30,
        });

        //Axe
        Items.Add(new AbilityData(AbilityType.Axe)
        {
            Description = "+25% for a 5 times",
            Cooldown = 30,
        });


    }

    public static AbilityData GetAbility(AbilityType id)
    {
        foreach (var itemData in Items)
        {
            if (itemData.Id == id)
                return itemData;
        }

        return null;
    }
}
