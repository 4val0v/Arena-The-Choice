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

        _screenManager.CharacterChangeScreen.GetComponent<SelectCharacter>().OnCharacterSelected += SelectClass;
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
        //fight
        Logger.Log("game started!");
    }

    void HandleOnItemEquipped(int playerId, int itemId)
    {
        //get item
        Logger.Log("ItemEquipped: playerId:" + playerId + ", itemId:" + itemId);
    }

    void HandleOnStepItemsReceived(EquipStep step, System.Collections.Generic.IEnumerable<int> items)
    {
        //create screen change item
        Logger.Log("Current step:" + step + ", items...");

		_screenManager.GetItem.UpdateStock(step, items);
    }

    void HandleOnFirstPlayerReceived(int playerId)
    {
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
                break;
            case NetStatus.ConnectingToBattle:
                _screenManager.ChangeScreen(ScreenManager.Screens.Connecting);
                break;
            case NetStatus.ConnectedToBattle:
                _screenManager.ChangeScreen(ScreenManager.Screens.CharacterChange);
                _client.UpdateName(_screenManager.MainScreen.GetComponent<MainScreen>().NameOfPlayer);
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
