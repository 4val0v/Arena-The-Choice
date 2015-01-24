using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class SelectCharacter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int CharacterIndex 
	{
		set;
		get;
	}

	public void GetIt()
	{
		if (OnCharacterSelected != null)
		{
			OnCharacterSelected.Invoke(CharacterIndex);
		}
	}

	[SerializeField]
	private List<Button> _buttons;

	public Action<int> OnCharacterSelected;
}
