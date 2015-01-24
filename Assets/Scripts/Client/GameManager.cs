using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        _client = PunNetClient.Instance;
        _client.OnStatusChanged += HandleOnStatusChanged;
        _client.OnEnemyNameUpdated += HandleOnEnemyNameUpdated;
        _client.OnEnemyClassUpdated += HandleOnEnemyClassUpdated;
        _client.OnClassUpdated += HandleOnClassUpdated;
        _client.OnFirstPlayerReceived += HandleOnFirstPlayerReceived;
        _client.OnStepItemsReceived += HandleOnStepItemsReceived;
        _client.OnItemEquipped += HandleOnItemEquipped;
        _client.OnGameStarted += HandleOnGameStarted;
        _client.OnGameFinished += HandleOnGameFinished;
        _client.OnNameUpdated += HandleOnNameUpdated;

		_screenManager.CharacterChangeScreen.OnCharacterSelected += SelectClass;
        _screenManager.GetItem.ItemEquipClicked += GetItemOnItemEquipClicked;
    }

    void Start()
    {
        _client.Connect();
    }

    void HandleOnGameFinished(int playrWinID)
    {
        //onPlayerWin;
        Logger.Log("game finish. winner id:" + playrWinID);
    }

    void HandleOnGameStarted()
    {
        Logger.Log("Game started!");

        _screenManager.ChangeScreen(ScreenManager.Screens.Arena);
    }

    void HandleOnItemEquipped(int playerId, int itemId)
    {
        //get item
        Logger.Log("ItemEquipped: playerId:" + playerId + ", itemId:" + itemId);

        //refresh
        _screenManager.GetItem.UpdateEquippedItems(_client.PlayerData.EquippedItems, _client.EnemyData.EquippedItems);

        _screenManager.GetItem.SetTurn(playerId != _client.PlayerData.Id);
    }

    void HandleOnStepItemsReceived(EquipStep step, System.Collections.Generic.IEnumerable<int> items)
    {
        //create screen change item
        Logger.Log("Current step:" + step + ", items...");

        _screenManager.GetItem.UpdateStock(step, items);

        var my = _screenManager.GetItem.FirstPlayerId == _client.PlayerData.Id
            ? (step == EquipStep.FirstHand || step == EquipStep.Armor)
            : step == EquipStep.SecondHand;

        _screenManager.GetItem.SetTurn(my);

        _screenManager.GetItem.UpdateSelection();
    }

    void HandleOnFirstPlayerReceived(int playerId)
    {
        Logger.Log("I'm first:" + (_client.PlayerData.Id == playerId) + ", fisrtId:" + playerId);

        _screenManager.GetItem.SetFirstPlayerId(playerId);
        _screenManager.GetItem.IsYouFirst = _client.PlayerData.Id == playerId;
    }

    void HandleOnEnemyClassUpdated(CharacterClass classId)
    {
        CheckLastSelectionOfCharacter();
        //change enemy character 
        Logger.Log("enemy classId:" + classId);
    }

    void HandleOnClassUpdated(CharacterClass obj)
    {
        CheckLastSelectionOfCharacter();
    }

    void HandleOnNameUpdated(string name)
    {
        _screenManager.TopBar.UpdatePlayerName(name);
    }

    void HandleOnEnemyNameUpdated(string name)
    {
        _screenManager.TopBar.UpdateEnemyName(name);
    }

    private void GetItemOnItemEquipClicked(int i)
    {
        _client.EquipItem(i);

        if (_client.PlayerData.EquippedItems.Count == _client.EnemyData.EquippedItems.Count &&
            _client.PlayerData.EquippedItems.Count == 3)
        {
            //START GAME!!!
            _screenManager.GetItem.SetTurn(false);
        }
        else
        {
            //_screenManager.GetItem.SetTurn(!_screenManager.GetItem.IsMyTurn);
        }
    }

    private void CheckLastSelectionOfCharacter()
    {
        if (_client.PlayerData.Class != CharacterClass.None && _client.EnemyData.Class != CharacterClass.None)
        {
            _screenManager.ChangeScreen(ScreenManager.Screens.GetItem);
        }
    }

    void HandleOnStatusChanged(NetStatus status)
    {
        Logger.Log("status changed:" + status);
        switch (status)
        {
            case NetStatus.Connected:
                _screenManager.ChangeScreen(ScreenManager.Screens.Main);
                break;
            case NetStatus.Disconnected:
                _screenManager.ChangeScreen(ScreenManager.Screens.Main);

                CoroutineExecuter.Execute(_client.Connect);
                break;
            case NetStatus.ConnectingToBattle:
                _screenManager.ChangeScreen(ScreenManager.Screens.Connecting);
                break;
            case NetStatus.ConnectedToBattle:
                _screenManager.ChangeScreen(ScreenManager.Screens.CharacterChange);
                _client.UpdateName(_screenManager.MainScreen.GetComponent<MainScreen>().NameOfPlayer);

                Logger.Log("myId:" + _client.PlayerData.Id + ", enemyId:" + _client.EnemyData.Id);
                break;
            default:
                break;
        }
    }

    public void ConnectToBattle(bool isBot)
    {
        _client.CreateOrJoinToBattle(isBot ? GameMode.PvE : GameMode.PvP);
    }

    public void SelectClass(int num)
    {
        _screenManager.SetCustomText(Texts.PLAYER_WAIT);
        _screenManager.ChangeScreen(ScreenManager.Screens.Connecting);
        _client.SetClass((CharacterClass)num);
    }

    private INetClient _client;
    [SerializeField]
    private ScreenManager _screenManager;
}
