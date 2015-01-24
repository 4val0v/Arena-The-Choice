using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        _client = PunNetClient.Instance;
        _client.OnStatusChanged += HandleOnStatusChanged;
        _client.OnEnemyNameUpdated += HandleOnEnemyNameUpdated;
        _client.OnEnemyClassUpdated += HandleOnEnemyClassUpdated;
        _client.OnFirstPlayerReceived += HandleOnFirstPlayerReceived;
        _client.OnStepItemsReceived += HandleOnStepItemsReceived;
        _client.OnItemEquipped += HandleOnItemEquipped;
        _client.OnGameStarted += HandleOnGameStarted;
        _client.OnGameFinished += HandleOnGameFinished;

		_screenManager.CharacterChangeScreen.GetComponent<SelectCharacter> ().OnCharacterSelected += SelectClass;
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
    }

    void HandleOnFirstPlayerReceived(int playerId)
    {
        //id of first 
        Logger.Log("first player:" + playerId);
    }

    void HandleOnEnemyClassUpdated(CharacterClass classId)
    {
        //change enemy character 
        Logger.Log("enemy classId:" + classId);
    }

    void HandleOnEnemyNameUpdated(string name)
    {
        //update enemy name;
        Logger.Log("enemy name:" + name);
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

                _playerName = "Pl" + UnityEngine.Random.Range(0, short.MaxValue);
                _client.UpdateName(_playerName);
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
		_screenManager.ConnectingScreen.GetComponent<Connecting> ().ChangeMainWord (Texsts.PLAYER_WAIT);
		_screenManager.ChangeScreen(ScreenManager.Screens.Connecting);
		_client.SetClass((CharacterClass) num);
	}

    private string _playerName = "";

    private INetClient _client;
    [SerializeField]
    private ScreenManager _screenManager;
}
