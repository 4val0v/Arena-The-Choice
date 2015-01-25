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
            Description = "Main. Quickly triple damage",
            Cooldown = 10,
        });

        //Mace
        Items.Add(new AbilityData(AbilityType.Mace)
        {
            Description = "Main. One strong attack +230% Dmg",
            Cooldown = 10,
        });

        //Sword
        Items.Add(new AbilityData(AbilityType.Sword)
        {
            Description = "Main. Slowing enemy attacks by 50% for 2 attack",
            Cooldown = 10,
        });

        //Axe
        Items.Add(new AbilityData(AbilityType.Axe)
        {
            Description = "Main. Next 5 attack inscreased by 25% DMG",
            Cooldown = 10,
        });

        //dagger
        Items.Add(new AbilityData(AbilityType.Dagger)
        {
            Description = "An open wound. Enemy receives 20 dmg per their attack.\n('Heal' takes effect)",
            Cooldown = 25,
        });

        //shield
        Items.Add(new AbilityData(AbilityType.Shield)
        {
            Description = "Block 15 dmg per enemy attack.\n(Main skills takes effect)",
            Cooldown = 15,
        });

        //shield
        Items.Add(new AbilityData(AbilityType.BigShield)
        {
            Description = "Block 20 dmg per enemy attack.\n(Main skills takes effect)",
            Cooldown = 15,
        });

        //helm
        Items.Add(new AbilityData(AbilityType.Helm)
        {
            Description = "'Heal'. Healing +50HP per 5 your attacks",
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

        throw new Exception();
    }
}
