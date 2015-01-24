using System;
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
            Cooldown = 10,
        });

        //Mace
        Items.Add(new AbilityData(AbilityType.Mace)
        {
            Description = "+200% Dmg",
            Cooldown = 10,
        });

        //Sword
        Items.Add(new AbilityData(AbilityType.Sword)
        {
            Description = "-20% Attack speed",
            Cooldown = 10,
        });

        //Axe
        Items.Add(new AbilityData(AbilityType.Axe)
        {
            Description = "+25% for a 5 times",
            Cooldown = 10,
        });

        //dagger
        Items.Add(new AbilityData(AbilityType.Dagger)
        {
            Description = "Enemy take 20 dmg when he attacked you 5 times!",
            Cooldown = 8,
        });

        //shield
        Items.Add(new AbilityData(AbilityType.Shield)
        {
            Description = "Add 20 defense to you when enemy attack you 5 times",
            Cooldown = 8,
        });

        //helm
        Items.Add(new AbilityData(AbilityType.Helm)
        {
            Description = "+100HP every you 5 attacks",
            Cooldown = 20,
        });
    }

    public static AbilityData GetAbility(AbilityType id)
    {
        foreach (var itemData in Items)
        {
            if (itemData.Id == id)
                return itemData;
        }

        throw new Exception();
    }
}
