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

    public GameObject CharacterChangeScreen
    {
        get
        {
            return _characterChangeScreen;
        }
    }

    public GameObject ConnectingScreen
    {
        get
        {
            return _connecting;
        }
    }

    public GameObject MainScreen
    {
        get
        {
            return _mainScreen;
        }
    }

    public GameObject GetItem
    {
        get
        {
            return _getItem;
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
        ConnectingScreen.GetComponent<Connecting>().ChangeMainWord(text);
    }

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

    private GameObject _currentScreen;

    public enum Screens
    {
        Main,
        CharacterChange,
        Connecting,
        GetItem
    }
}
