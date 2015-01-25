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
                _topBar.SetItemsIconsLeft(null, null, null);
                _topBar.SetItemsIconsRight(null, null, null);
                _topBar.Show();
                _topBar.SwitchStat(true);
                _currentScreen = _getItem;
                break;
            case Screens.Arena:
                _topBar.SwitchStat(false);
                _topBar.Show();
                _currentScreen = _arena;
                break;
            case Screens.FightFinished:
                _topBar.Hide();
                _currentScreen = _fightFinished;
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
        yield return _fightFinished;
    }

    public SelectCharacter CharacterChangeScreen
    {
        get
        {
            if (_selectCharacterComp == null)
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

    public Arena Arena
    {
        get
        {
            if (_arenaComp == null)
            {
                _arenaComp = _arena.GetComponent<Arena>();
            }
            return _arenaComp;
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

    public FightFinished FightFinished
    {
        get
        {
            if (_fightFinishedComp == null)
            {
                _fightFinishedComp = _fightFinished.GetComponent<FightFinished>();
            }
            return _fightFinishedComp;
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
    private Arena _arenaComp;
    private FightFinished _fightFinishedComp;

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

    [SerializeField]
    private GameObject _fightFinished;

    private GameObject _currentScreen;

    public enum Screens
    {
        Main,
        CharacterChange,
        Connecting,
        GetItem,
        Arena,
        FightFinished,
    }
}
