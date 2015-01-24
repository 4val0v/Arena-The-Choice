using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    public void ChangeScreen(Screens screen)
    {
        _topBar.Hide();

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
                _topBar.Show();
                _currentScreen = _getItem;
				break;
			case Screens.Arena:
				_topBar.Show();
				_currentScreen = _arena;
                break;
        }
        _currentScreen.SetActive(true);
    }

    private void HideAll()
    {
        foreach (var screen in AllScreens())
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

	public SelectCharacter CharacterChangeScreen
    {
        get
        {
			if(_selectCharacterComp == null)
			{
				_selectCharacterComp = _characterChangeScreen.GetComponent<SelectCharacter>();
			}
			return _selectCharacterComp;
        }
    }

    public Connecting ConnectingScreen
    {
        get
        {
			if (_connectingComp == null)
			{
				_connectingComp = _connecting.GetComponent<Connecting>();
			}
			return _connectingComp;
        }
    }

    public GameObject MainScreen
    {
        get
        {
            return _mainScreen;
        }
    }

    public GetItem GetItem
    {
        get
        {
			if (_getItemComp == null)
			{
				_getItemComp = _getItem.GetComponent<GetItem>();
			}
			return _getItemComp;
		}
	}

    public TopBar TopBar
    {
        get
        {
            return _topBar;
        }
    }

    public void SetCustomText(string text)
    {
        ConnectingScreen.ChangeMainWord(text);
    }

	private GetItem _getItemComp;
	private Connecting _connectingComp;
	private SelectCharacter _selectCharacterComp;

    [SerializeField]
    private GameObject _mainScreen;

    [SerializeField]
    private GameObject _characterChangeScreen;

    [SerializeField]
    private GameObject _connecting;

    [SerializeField]
    private GameObject _getItem;

    [SerializeField]
    private TopBar _topBar;

	[SerializeField]
	private GameObject _arena;

    private GameObject _currentScreen;

    public enum Screens
    {
        Main,
        CharacterChange,
        Connecting,
        GetItem,
		Arena
    }
}
