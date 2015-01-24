using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string NameOfPlayer 
	{
		get 
		{
			return _inputField.text;
		}
	}

	[SerializeField]
	private Text _inputField;
}
