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
    }

    void Start()
    {
        _client.Connect();
    }

    void HandleOnGameFinished(int playrWinID)
    {
        //onPlayerWin;
    }

    void HandleOnGameStarted()
    {
        //fight
    }

    void HandleOnItemEquipped(int playerId, int itemId)
    {
        //get item
    }

    void HandleOnStepItemsReceived(EquipStep step, System.Collections.Generic.IEnumerable<int> items)
    {
        //create screen change item
    }

    void HandleOnFirstPlayerReceived(int playerId)
    {
        //id of first 
    }

    void HandleOnEnemyClassUpdated(CharacterClass classId)
    {
        //change enemy character 
    }

    void HandleOnEnemyNameUpdated(string name)
    {
        //update enemy name;
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
                break;
            case NetStatus.ConnectedToBattle:
                _screenManager.ChangeScreen(ScreenManager.Screens.CharacterChange);
                break;
            default:
                break;
        }
    }

    public void ConnectToBattle(bool isBot)
    {
        _playerName = "Pl" + UnityEngine.Random.Range(0, short.MaxValue);
        _client.UpdateName(_playerName);
        _screenManager.ChangeScreen(ScreenManager.Screens.Connecting);
        _client.CreateOrJoinToBattle(isBot ? GameMode.PvE : GameMode.PvP);
    }

    private string _playerName = "";

    private INetClient _client;
    [SerializeField]
    private ScreenManager _screenManager;
}
