using System.Collections.Generic;

public static class AbilitiesProvider
{
    public static readonly List<AbilityData> Items;

    static AbilitiesProvider()
    {
        Items = new List<AbilityData>();

    }

    public static AbilityData GetAbility(int id)
    {
        foreach (var itemData in Items)
        {
            if (itemData.Id == id)
                return itemData;
        }

        return null;
    }
}
