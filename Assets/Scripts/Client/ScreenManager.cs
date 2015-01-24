using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour 
{

	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

	public void ChangeScreen(Screens screen)
	{
		if (_currentScreen != null)
		{
			_currentScreen.SetActive(false);
		}
		switch (screen) 
		{
			case Screens.CharacterChange:
				_currentScreen = _characterChangeScreen;
				break;
			case Screens.Main:
				_currentScreen = _mainScreen;
				break;
		}
		_currentScreen.SetActive(true);
	}

	public GameObject MainScr {
		get {
			return _mainScreen;
		}
	}

	public GameObject CharactersScr {
		get {
			return _characterChangeScreen;
		}
	}

	private void HideAll()
	{
		foreach (var screen in AllScreens ()) 
		{
			screen.SetActive(false);
		}
	}

	private IEnumerable<GameObject> AllScreens()
	{
		yield return _mainScreen;
		yield return _characterChangeScreen;
	}


	[SerializeField]
	private GameObject _mainScreen;

	[SerializeField]
	private GameObject _characterChangeScreen;

	private GameObject _currentScreen;

	public enum Screens
	{
		Main,
		CharacterChange
	}
}
