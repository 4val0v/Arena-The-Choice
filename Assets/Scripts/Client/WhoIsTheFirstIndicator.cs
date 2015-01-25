using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class WhoIsTheFirstIndicator : MonoBehaviour 
{

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}

	public void Show()
	{
		gameObject.SetActive (true);
	}

	public void Hide()
	{
		gameObject.SetActive (false);
	}

	public void Play(string name)
	{
		_name = name;
		_whoIsTheFirst.Play (name);
	}
	
	private void OnEndAnimChangeFirst()
	{
		if (OnEndAnimChoise != null)
		{
			OnEndAnimChoise.Invoke();
		}
	}

	private void ChangeText()
	{
		if (_name == "you")
		{
			_text.text = Texts.COIN_FIRST;
		}
		else
		{
			_text.text = Texts.COIN_SECOND;
		}
	}

	void OnEnable()
	{
		_text.text = Texts.COIN;
	}


	[SerializeField]
	private Animator _whoIsTheFirst;

	[SerializeField]
	private Text _text;

	private string _name;

	public Action OnEndAnimChoise;
}
