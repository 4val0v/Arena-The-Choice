using System.Collections.Generic;

public static class ItemsProvider
{
    public static readonly List<ItemData> Items;

    static ItemsProvider()
    {
        Items = new List<ItemData>();

        //left hand
        Items.Add(new ItemData(1, AbilityType.Sword)
                  {
                      //Меч
                      Type = ItemType.LeftHand,
                      Dmg = 50,
                      AttackSpeed = 60,
                      Accuracy = 0.9f,
                  });
        Items.Add(new ItemData(2, AbilityType.Axe)
                  {
                      //Топор
                      Type = ItemType.LeftHand,
                      Dmg = 60,
                      AttackSpeed = 50,
                      Accuracy = 0.91f, //да, точно 0,91
                  });
        Items.Add(new ItemData(3, AbilityType.Mace)
                  {
                      //булова
                      Type = ItemType.LeftHand,
                      Dmg = 70,
                      AttackSpeed = 43,
                      Accuracy = 0.9f,
                  });
        Items.Add(new ItemData(4, AbilityType.Spear)
                  {
                      //Копье
                      Type = ItemType.LeftHand,
                      Dmg = 40,
                      AttackSpeed = 75,
                      Accuracy = 0.9f,
                  });

        //right hand
        Items.Add(new ItemData(5, AbilityType.Dagger)
                  {
                      //Кинжал
                      Type = ItemType.RightHand,
                      Dmg = 15,
                  });
        Items.Add(new ItemData(6, AbilityType.Dagger)
                  {
                      //Маленький меч
                      Type = ItemType.RightHand,
                      Dmg = 20,
                      AttackSpeed = (-5)
                  });

        Items.Add(new ItemData(7, AbilityType.BigShield)
                  //Большой щит
                  {
                      Type = ItemType.RightHand,
                      AttackSpeed = (-5)
                  });

        Items.Add(new ItemData(8, AbilityType.Shield)
                  //маленький щит
                  {
                      Type = ItemType.RightHand,
                  });

        //helm
        Items.Add(new ItemData(9, AbilityType.Helm)
                  {
                      //шлем с рогами
                      Type = ItemType.Helm,
                  });
        Items.Add(new ItemData(10, AbilityType.Helm)
                  {
                      //шлем крестоносца
                      Type = ItemType.Helm,
                  });
        Items.Add(new ItemData(11, AbilityType.Helm)
                  {
                      //шлем с пером
                      Type = ItemType.Helm,
                  });
        Items.Add(new ItemData(12, AbilityType.Helm)
                  {
                      //шлем с копьем (или другой любой)
                      Type = ItemType.Helm,
                  });
    }

    public static ItemData GetItem(int id)
    {
        foreach (var itemData in Items)
        {
            if (itemData.Id == id)
                return itemData;
        }

        return null;
    }
}
