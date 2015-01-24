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
    }

    private void EndFirstPlayerAnim()
    {
        Logger.Log("End First playerAnim");
        _whoIsTheFirst.Hide();

        _stockGrid.transform.parent.gameObject.SetActive(true);
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

            newView.transform.localScale = Vector3.one;

            _stockItems.Add(script);
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
