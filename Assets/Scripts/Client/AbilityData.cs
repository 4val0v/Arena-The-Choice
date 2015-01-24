using UnityEngine;

public class AbilityData
{
    public int Id { get; private set; }

    public string Name { get { return "Ab" + Id; } }

    public string Description { get; set; }

    public float Cooldown { get; set; }

    private string IconPath { get { return "/AbilityIcons/" + Id; } }

    public Sprite Icon
    {
        get
        {
            var icon = Resources.Load<Sprite>(IconPath);
            return icon;
        }
    }

    public AbilityData(int id)
    {
        Id = id;
    }
}
