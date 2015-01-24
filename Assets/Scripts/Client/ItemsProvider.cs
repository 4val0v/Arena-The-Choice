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
			//Меч
			Type = ItemType.LeftHand,
			Dmg = 50,
			AttackSpeed = 60,
			Accuracy = 0.9f,
		});
		Items.Add(new ItemData(2)
		          {
			//Топор
			Type = ItemType.LeftHand,
			Dmg = 60,
			AttackSpeed = 50,
			Accuracy = 0.91f, //да, точно 0,91
		});
		Items.Add(new ItemData(3)
		          {
			//булова
			Type = ItemType.LeftHand,
			Dmg = 70,
			AttackSpeed = 43,
			Accuracy = 0.9f,
		});
		Items.Add(new ItemData(4)
		          {
			//Копье
			Type = ItemType.LeftHand,
			Dmg = 40,
			AttackSpeed = 75,
			Accuracy = 0.9f,
		});

		//right hand
		Items.Add(new ItemData(5)
		          {
			//Кинжал
			Type = ItemType.RightHand,
			Dmg = 20,
			AttackSpeed = +0
		});
		Items.Add(new ItemData(6)
		          {
			//Маленький меч
			Type = ItemType.RightHand,
			Dmg = 30,
			AttackSpeed = (-5)
		});
		Items.Add(new ItemData(7)
		          //Большой щит
		          {
			Type = ItemType.RightHand,
			Defense = 40,
			AttackSpeed = (-10)
		});
		Items.Add(new ItemData(8)
		          //маленький щит
		          {
			Type = ItemType.RightHand,
			Defense = 25,
			AttackSpeed = (-5)
		});

		//helm
		Items.Add(new ItemData(9)
		          {
			//шлем с рогами
			Type = ItemType.Helm,
			Defense = 10,
			AttackSpeed=0
		});
		Items.Add(new ItemData(10)
		          {
			//шлем крестоносца
			Type = ItemType.Helm,
			Defense = 15,
			AttackSpeed=-5
		});
		Items.Add(new ItemData(11)
		          {
			//шлем с пером
			Type = ItemType.Helm,
			Defense = 8,
			AttackSpeed=+5
		});
		Items.Add(new ItemData(12)
		          {
			//шлем с копьем (или другой любой)
			Type = ItemType.Helm,
			Defense = 12,
			AttackSpeed=0
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
