using System.Collections.Generic;

public static class ItemsProvider
{
    public static readonly List<ItemData> Items;

    static ItemsProvider()
    {
        Items = new List<ItemData>();

        //left hand
        Items.Add(new ItemData(1)
        {
            Type = ItemType.LeftHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });
        Items.Add(new ItemData(2)
        {
            Type = ItemType.LeftHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });
        Items.Add(new ItemData(3)
        {
            Type = ItemType.LeftHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });
        Items.Add(new ItemData(4)
        {
            Type = ItemType.LeftHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });

        //right hand
        Items.Add(new ItemData(5)
        {
            Type = ItemType.RightHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });
        Items.Add(new ItemData(6)
        {
            Type = ItemType.RightHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });
        Items.Add(new ItemData(7)
        {
            Type = ItemType.RightHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });
        Items.Add(new ItemData(8)
        {
            Type = ItemType.RightHand,
            Dmg = 1,
            AttackSpeed = 1,
            Accuracy = 0.95f,
        });

        //helm
        Items.Add(new ItemData(9)
        {
            Type = ItemType.Helm,
            Defense = 10,
        });
        Items.Add(new ItemData(10)
        {
            Type = ItemType.Helm,
            Defense = 10,
        });
        Items.Add(new ItemData(11)
        {
            Type = ItemType.Helm,
            Defense = 10,
        });
        Items.Add(new ItemData(12)
        {
            Type = ItemType.Helm,
            Defense = 10,
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
