using System.Collections.Generic;
using UnityEngine;
using System.Collections;
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

    public void UpdateStock(IEnumerable<int> items)
    {
        //clear all
        ClearStock();

}
