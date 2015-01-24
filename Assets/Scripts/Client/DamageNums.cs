using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageNums : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void EndAnim()
	{
		gameObject.Recycle();
	}

	public void SetText(string text, Color color)
	{
		_text.text = text;
		_text.color = color;
	}

	[SerializeField]
	private Text _text;
}
