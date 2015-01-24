﻿using System.Collections.Generic;

/// <summary>
/// Only Data! No logic
/// </summary>
public class PlayerData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CharacterClass Class { get; set; }
    public readonly List<int> EquippedItems = new List<int>();

    public CharacterData BaseData { get { return CharacterDataProviders.GetBaseData(Class); } }

    public float Def
    {
        get
        {
            var par = BaseData.BaseDef;
            foreach (var item in EquippedItems)
            {
                par += ItemsProvider.GetItem(item).Defense;
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
