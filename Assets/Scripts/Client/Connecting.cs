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
		if (!_tempChange)
		{
			ChangeMainWord(Texts.CONNECTING);
		}
		_tempChange = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(enabled)
		{
			_time += Time.deltaTime;
			if (_time > 1)
			{
				_time = 0;
				if (_added.Length == 3)
				{
					_added = ".";
				}
				else
				{
					_added += ".";
				}
				_textField.text = _mainWord + _added;
			}
		}
	}

	public void ChangeMainWord(string word)
	{
		_mainWord = word;
		_tempChange = true;
		_textField.text = _mainWord;
		_added = ".";
	}

	private float _time;
	private string _mainWord;
	private bool _tempChange = false;
	private string _added = ".";

	[SerializeField]
	private Text _textField;


}
