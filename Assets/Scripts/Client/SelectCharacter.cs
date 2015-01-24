using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

	private void GetIt()
	{
		//Character was selected
	}

	[SerializeField]
	private List<Button> _buttons;
}
