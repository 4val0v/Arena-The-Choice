using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public enum ItemType
{
    LeftHand,
    RightHand,
    Helm
}

public class ItemData
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ItemType Type { get; set; }

    public float Dmg { get; set; }
    public float Defense { get; set; }
    public float AttackSpeed { get; set; }

    /// <summary>
    /// Точность
    /// </summary>
    public float Accuracy { get; set; }

    public AbilityType AbilityId { get; private set; }

    private string IconPath { get { return "ItemIcons/" + Id; } }

    public Sprite Icon
    {
        get
        {
            var icon = Resources.Load<Sprite>(IconPath);
            if (icon == null)
                throw new System.Exception();

            return icon;
        }
    }

    private string ItemPath
    {
        get { return "ItemViews/" + Id; }
    }

    public Sprite ItemView
    {
        get
        {
            var itemView = Resources.Load<Sprite>(ItemPath);

            if (itemView == null)
                throw new System.Exception();

            return itemView;
        }
    }

    public AbilityData Ability { get { return AbilitiesProvider.GetAbility(AbilityId); } }

    public ItemData(int id, AbilityType ability)
    {
        Id = id;
        AbilityId = ability;
    }
}
