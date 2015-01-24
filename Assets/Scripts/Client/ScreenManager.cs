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
			case Screens.Connecting:
				_currentScreen = _connecting;
				break;
			case Screens.GetItem:
				_currentScreen = _getItem;
				break;
		}
		_currentScreen.SetActive(true);
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
		yield return _connecting;
		yield return _getItem;
	}

	public GameObject CharacterChangeScreen 
	{
		get {
			return _characterChangeScreen;
		}
	}

	public GameObject ConnectingScreen {
		get {
			return _connecting;
		}
	}

	public void SetCustomText(string text)
	{
		ConnectingScreen.GetComponent<Connecting> ().ChangeMainWord (text);
	}

	[SerializeField]
	private GameObject _mainScreen;

	[SerializeField]
	private GameObject _characterChangeScreen;

	[SerializeField]
	private GameObject _connecting;

	[SerializeField]
	private GameObject _getItem;

	private GameObject _currentScreen;

	public enum Screens
	{
		Main,
		CharacterChange,
		Connecting,
		GetItem
	}
}
