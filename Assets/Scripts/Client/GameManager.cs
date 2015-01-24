using UnityEngine;
using System.Collections;

public class GameManager:MonoBehaviour
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
		_client.Connect ();
	}

	void HandleOnGameFinished (int playrWinID)
	{
		//onPlayerWin;
	}

	void HandleOnGameStarted ()
	{
		//fight
	}

	void HandleOnItemEquipped (int playerId, int itemId)
	{
		//get item
	}

	void HandleOnStepItemsReceived (EquipStep step, System.Collections.Generic.IEnumerable<int> items)
	{
		//create screen change item
	}

	void HandleOnFirstPlayerReceived (int playerId)
	{
		//id of first 
	}

	void HandleOnEnemyClassUpdated (CharacterClass classId)
	{
		//change enemy character 
	}

	void HandleOnEnemyNameUpdated (string name)
	{
		//update enemy name;
	}

	void HandleOnStatusChanged (NetStatus status)
	{
		switch (status) {
		case NetStatus.Connected:
			// first screen;
			break;
		case NetStatus.Disconnected:
			//connect;to fist screen
			break;
		case NetStatus.ConnectingToBattle:
			//mod
			break;
		case NetStatus.ConnectedToBattle:
			break;
		default:
						break;
		}
	}

	private INetClient _client;
}
