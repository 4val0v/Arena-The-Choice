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

    [SerializeField]
    private Text _knightText;

    [SerializeField]
    private GameObject _knightObj;

    public readonly List<StockItemView> StockItems = new List<StockItemView>();

    public event Action<int> ItemEquipClicked = delegate { };
    public event Action<int> ItemSelected = delegate { };

    public bool IsMyTurn { get; private set; }

    public int FirstPlayerId { get; private set; }

    private INetClient Client { get { return PunNetClient.Instance; } }

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

        SetTurn(false);
    }

    private void EndFirstPlayerAnim()
    {
        Logger.Log("End First playerAnim");
        _whoIsTheFirst.Hide();

        _stockGrid.transform.parent.gameObject.SetActive(true);

        foreach (var view in StockItems)
        {
            view.IsEquipped = false;
            view.IsSelected = false;
        }

        SetTurn(IsYouFirst);

        SetKnightVisible(false);
    }

    public void SetFirstPlayerId(int playerId)
    {
        FirstPlayerId = playerId;
    }

    public void SetTurn(bool isMyTurn)
    {
        IsMyTurn = isMyTurn;

        UpdateSelection();
    }

    public void UpdateSelection()
    {
        //select any
        if (!StockItems.Any(n => n.IsSelected))
        {
            OnItemClicked(StockItems[0]);
        }

        if (!IsMyTurn)
        {
            EquipButtonVisible(false);
        }
        else
        {
            var selectedItem = StockItems.First(n => n.IsSelected);

            EquipButtonVisible(!selectedItem.IsEquipped);
        }
    }

    public void SetKnightVisible(bool visible)
    {
        _knightObj.SetActive(visible);
    }

    public void SetDescriptionVisible(bool visible)
    {
        _itemDescriptionText.gameObject.SetActive(visible);
    }

    public void UpdateKnightText(string text)
    {
        _knightText.text = text;
    }

    public void UpdateStock(EquipStep step, IEnumerable<int> items)
    {
        _equipStepText.text = Texts.StepsNames[step];

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

        foreach (var view in StockItems)
        {
            view.IsEquipped = false;
            view.IsSelected = false;
        }
    }

    public void UpdateEquippedItems(IEnumerable<int> myItems, IEnumerable<int> enemyItems)
    {
        foreach (var child in StockItems)
        {
            child.IsEquipped = myItems.Any(n => n == child.ItemData.Id) || enemyItems.Any(n => n == child.ItemData.Id);
        }
    }

    public void UpdateItemDescription(string text)
    {
        _itemDescriptionText.text = text;
    }

    private void ClearStock()
    {
        foreach (var child in StockItems)
        {
            child.ItemClicked -= OnItemClicked;
            DestroyObject(child.gameObject);
        }

        StockItems.Clear();

        UpdateItemDescription("");
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
        //Logger.Log("OnItemClicked:" + view.ItemData.Id);

        foreach (var child in StockItems)
        {
            child.IsSelected = view.ItemData.Id == child.ItemData.Id;
        }

        UpdateItemDescription(GetDescriptionText(view.ItemData));

        ItemSelected(view.ItemData.Id);

        EquipButtonVisible(view.IsSelected && !view.IsEquipped && IsMyTurn);
    }

    private string GetDescriptionText(ItemData item)
    {
        string txt = "";

        switch (item.Type)
        {
            case ItemType.LeftHand:
                txt += "Dmg:" + item.Dmg + "\n";
                txt += "AS:" + item.AttackSpeed + "\n";
                txt += "Accur:" + item.Accuracy;
                break;

            case ItemType.RightHand:
                if (item.Defense > 0.001f)
                {
                    txt += "Def:" + item.Defense;
                }
                else
                {
                    txt += "Dmg:" + item.Dmg + "\n";
                    txt += "AS:" + item.AttackSpeed + "\n";
                    txt += "Accur:" + item.Accuracy;
                }
                break;

            case ItemType.Helm:
                txt += "Def:" + item.Defense;
                break;
        }

        if (item.Ability != null)
            txt += "\n" + GetAbilityDescription(item.Ability);

        return txt;
    }

    private string GetAbilityDescription(AbilityData data)
    {
        string txt = "";

        txt = data.Description;

        return txt;
    }

    private void EquipButtonVisible(bool visible)
    {
        //Logger.Log("equipbuttonVisible:" + visible);

        _equipItemBtn.SetActive(visible);
    }
}
