using System.Collections.Generic;

/// <summary>
/// Only Data! No logic
/// </summary>
public class PlayerData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CharacterClass Class { get; set; }
    public readonly List<int> EquippedItems = new List<int>();

    public readonly List<BaseAbility> Abilities = new List<BaseAbility>();

    public CharacterData BaseData { get { return CharacterDataProviders.GetBaseData(Class); } }

    private float _hp;

    public float CurrentHp
    {
        get { return _hp; }
        set
        {
            _hp = value;

            if (_hp < 0)
                _hp = 0;
        }
    }

    public float MaxHp
    {
        get { return BaseData.BaseHp; }
    }

    public float Def
    {
        get
        {
            var par = BaseData.BaseDef;

            foreach (var item in EquippedItems)
            {
                par += ItemsProvider.GetItem(item).Defense;
            }

            foreach (var ability in Abilities)
            {
                par += ability.Def;
            }

            return par;
        }
    }

    public float Dmg
    {
        get
        {
            var par = BaseData.BaseDmg;

            foreach (var item in EquippedItems)
            {
                par += ItemsProvider.GetItem(item).Dmg;
            }

            foreach (var ability in Abilities)
            {
                par += ability.Dmg;
            }

            return par;
        }
    }

    public float DmgWithoutAbilities
    {
        get
        {
            var par = BaseData.BaseDmg;

            foreach (var item in EquippedItems)
            {
                par += ItemsProvider.GetItem(item).Dmg;
            }

            return par;
        }
    }

    public float AttackSpeed
    {
        get
        {
            var par = BaseData.BaseAttackSpeed;

            foreach (var item in EquippedItems)
            {
                par += ItemsProvider.GetItem(item).AttackSpeed;
            }

            foreach (var ability in Abilities)
            {
                par += ability.AttackSpeed;
            }

            return par;
        }
    }

    public float Accuracy
    {
        get
        {
            var par = BaseData.BaseAccuracy;

            foreach (var item in EquippedItems)
            {
                par += ItemsProvider.GetItem(item).Accuracy;
            }

            return par;
        }
    }

}
