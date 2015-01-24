using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    [SerializeField]
    private WhoIsTheFirstIndicator _whoIsTheFirst;

    [SerializeField]
    private GameObject _stockItemPref;

    [SerializeField]
    private GameObject _stockGrid;

    [SerializeField]
    private Text _itemDescriptionText;

    [SerializeField]
    private Text _equipStepText;

    [SerializeField]
    private GameObject _equipItemBtn;

    public readonly List<StockItemView> StockItems = new List<StockItemView>();

    public event Action<int> ItemEquipClicked = delegate { };

    public bool IsMyTurn { get; private set; }

    void Start()
    {
        _whoIsTheFirst.OnEndAnimChoise += EndFirstPlayerAnim;
    }

    public bool IsYouFirst
    {
        get;
        set;
    }

    void OnEnable()
    {
        _stockGrid.transform.parent.gameObject.SetActive(false);

        _whoIsTheFirst.Show();
        if (IsYouFirst)
        {
            _whoIsTheFirst.Play("you");
        }
        else
        {
            _whoIsTheFirst.Play("enemy");
        }

        SetTurn(IsYouFirst);
    }

    private void EndFirstPlayerAnim()
    {
        Logger.Log("End First playerAnim");
        _whoIsTheFirst.Hide();

        _stockGrid.transform.parent.gameObject.SetActive(true);
    }

    public void SetTurn(bool isMyTurn)
    {
        IsMyTurn = isMyTurn;

        if (!isMyTurn)
        {
            EquipButtonVisible(false);
        }
        else
        {
            foreach (var view in StockItems)
            {
                if (!view.IsEquipped && !view.IsSelected)
                {
                    OnItemClicked(view);
                    break;
                }
            }
        }
    }

    public void UpdateStock(EquipStep step, IEnumerable<int> items)
    {
        _equipStepText.text = "Now step is " + step;

        Logger.Log("Update sctock:" + items.Count() + " items");

        //clear all
        ClearStock();

        //create item
        foreach (var itemId in items)
        {
            var itemData = ItemsProvider.GetItem(itemId);

            var newView = Instantiate(_stockItemPref) as GameObject;

            newView.transform.SetParent(_stockGrid.transform);

            var script = newView.GetComponent<StockItemView>();

            script.SetData(itemData);
            script.ItemClicked += OnItemClicked;

            newView.transform.localScale = Vector3.one;

            StockItems.Add(script);
        }
    }

    public void UpdateEquippedItems(IEnumerable<int> myItems, IEnumerable<int> enemyItems)
    {
        foreach (var child in StockItems)
        {
            child.IsEquipped = myItems.Any(n => n == child.ItemData.Id) || enemyItems.Any(n => n == child.ItemData.Id);
        }

        //refresh my inventory

        //refresh enemy inventory
    }

    private void ClearStock()
    {
        foreach (var child in StockItems)
        {
            child.ItemClicked -= OnItemClicked;
            DestroyObject(child.gameObject);
        }

        StockItems.Clear();
    }

    public void EquipItem()
    {
        var selectedItem = StockItems.FirstOrDefault(n => n.IsSelected && !n.IsEquipped);

        if (selectedItem == null)
            return;

        ItemEquipClicked(selectedItem.ItemData.Id);
    }

    private void OnItemClicked(StockItemView view)
    {
        Logger.Log("OnItemClicked:" + view.ItemData.Id);

        foreach (var child in StockItems)
        {
            child.IsSelected = view.ItemData.Id == child.ItemData.Id;
        }

        EquipButtonVisible(view.IsSelected && !view.IsEquipped && IsMyTurn);
    }

    private void EquipButtonVisible(bool visible)
    {
        Logger.Log("equipbuttonVisible:" + visible);

        _equipItemBtn.SetActive(visible);
    }
}
