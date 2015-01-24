using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Connecting : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

	}

	void OnEnable()
	{
		_time = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(enabled)
		{
			_time += Time.deltaTime;
			if (_time > 0.3)
			{
				_textField.text += ".";
			}
		}
	}

	private float _time;
	private string _mainWord = "Connecting";

	[SerializeField]
	private Text _textField;


}
