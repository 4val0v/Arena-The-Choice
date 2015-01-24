using System.Collections.Generic;
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

    private readonly List<StockItemView> _stockItems = new List<StockItemView>();

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
		_whoIsTheFirst.Show ();
        if (IsYouFirst)
        {
            _whoIsTheFirst.Play("you");
        }
        else
        {
            _whoIsTheFirst.Play("enemy");
        }
    }

	private void EndFirstPlayerAnim()
	{
		Logger.Log ("End First playerAnim");
		_whoIsTheFirst.Hide ();
	}

    public void UpdateStock(EquipStep step, IEnumerable<int> items)
    {
        //clear all
        ClearStock();

        //create item
        foreach (var itemId in items)
        {
            var itemData = ItemsProvider.GetItem(itemId);

            var newView = Instantiate(_stockItemPref) as GameObject;

            ((RectTransform)newView.transform).SetParent(_stockGrid.transform);

            _stockItems.Add(newView.GetComponent<StockItemView>());
        }
    }

    private void ClearStock()
    {
        foreach (var child in _stockItems)
        {
            DestroyObject(child.gameObject);
        }

        _stockItems.Clear();
    }

    public void SelectItem(int id)
    {

    }
}
