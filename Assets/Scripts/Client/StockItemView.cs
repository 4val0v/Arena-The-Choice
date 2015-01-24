using System;
using UnityEngine;
using UnityEngine.UI;

public class StockItemView : MonoBehaviour
{
    public Image IconImage;
    public Image SelectionImage;
    public Image EquippedImage;

    public ItemData ItemData { get; private set; }

    private bool _isSelected;

    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;

            SelectionImage.enabled = _isSelected;
        }
    }

    private bool _isEquipped;

    public bool IsEquipped
    {
        get { return _isEquipped; }
        set
        {
            _isEquipped = value;

            EquippedImage.enabled = _isEquipped;
        }
    }

    public event Action<StockItemView> ItemClicked = delegate { };

    public void SetData(ItemData data)
    {
        ItemData = data;

        IconImage.sprite = ItemData.Icon;
    }

    public void OnItemClick()
    {
        ItemClicked(this);
    }
}
