using System;
using UnityEngine;

public class AbilityData
{
    public AbilityType Id { get; private set; }

    public string Name { get { return "Ab" + Id; } }

    public string Description { get; set; }

    public float Cooldown { get; set; }

    private string IconPath { get { return "AbilityIcons/" + (int)Id; } }

    public Sprite Icon
    {
        get
        {
            var icon = Resources.Load<Sprite>(IconPath);

            if (icon == null)
                throw new Exception();

            return icon;
        }
    }

    public AbilityData(AbilityType id)
    {
        Id = id;
    }
}
