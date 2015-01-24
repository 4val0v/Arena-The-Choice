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
        _client.OnDmgReceived += HandleOnDmgReceived;
        _client.OnEnemyDmgReceived += HandleOnEnemyDmgReceived;
        _client.OnAbilityUsed += HandleOnAbilityUsed;

        _screenManager.CharacterChangeScreen.OnCharacterSelected += SelectClass;
        _screenManager.GetItem.ItemEquipClicked += GetItemOnItemEquipClicked;
        _screenManager.GetItem.ItemSelected += GetItemOnItemSelected;
    }

    private void GetItemOnItemSelected(int itemId)
    {
        var itemData = ItemsProvider.GetItem(itemId);

        var data = _client.PlayerData;
        var topBar = _screenManager.TopBar;

        if (data.Class != CharacterClass.None)
        {
            topBar.SetDmg(data.Dmg, itemData.Dmg);
            topBar.SetDef(data.Def, itemData.Defense);
            topBar.SetAttackSpeed(data.AttackSpeed, itemData.AttackSpeed);
        }
    }

    void Start()
    {
        _client.Connect();
    }

    void HandleOnGameFinished(int playrWinID)
    {
        //onPlayerWin;
        Logger.Log("game finish. winner id:" + playrWinID);

        _screenManager.FightFinished.SetWinner(_client.PlayerData.Id == playrWinID);
        _screenManager.ChangeScreen(ScreenManager.Screens.FightFinished);
    }

    void HandleOnGameStarted()
    {
        Logger.Log("Game started!");
        _screenManager.Arena.Client = _client;
        _screenManager.ChangeScreen(ScreenManager.Screens.Arena);
    }

    void HandleOnItemEquipped(int playerId, int itemId)
    {
        //get item
        Logger.Log("ItemEquipped: playerId:" + playerId + ", itemId:" + itemId);

        //refresh
        _screenManager.GetItem.UpdateEquippedItems(_client.PlayerData.EquippedItems, _client.EnemyData.EquippedItems);

        _screenManager.GetItem.SetTurn(playerId != _client.PlayerData.Id);

        _screenManager.TopBar.SetDmg(_client.PlayerData.Dmg);
        _screenManager.TopBar.SetDef(_client.PlayerData.Def);
        _screenManager.TopBar.SetAttackSpeed(_client.PlayerData.AttackSpeed);
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
        _client.EnemyData.CurrentHp = _client.EnemyData.MaxHp;
        _screenManager.TopBar.SetEnemyHp(1f);
    }

    void HandleOnClassUpdated(CharacterClass obj)
    {
        CheckLastSelectionOfCharacter();

        _screenManager.TopBar.SetDmg(_client.PlayerData.Dmg);
        _screenManager.TopBar.SetDef(_client.PlayerData.Def);
        _screenManager.TopBar.SetAttackSpeed(_client.PlayerData.AttackSpeed);

        _client.PlayerData.CurrentHp = _client.PlayerData.MaxHp;
        _screenManager.TopBar.SetPlayerHp(1f);
    }

    void HandleOnNameUpdated(string name)
    {
        _screenManager.TopBar.UpdatePlayerName(name);
    }

    void HandleOnEnemyNameUpdated(string name)
    {
        _screenManager.TopBar.UpdateEnemyName(name);
    }

    private void HandleOnEnemyDmgReceived(int weaponId, int dmg)
    {
        _client.EnemyData.CurrentHp -= dmg;

        _screenManager.TopBar.SetEnemyHp(_client.EnemyData.CurrentHp / _client.EnemyData.MaxHp);
    }

    private void HandleOnDmgReceived(int weaponId, int dmg)
    {
        _client.PlayerData.CurrentHp -= dmg;

        _screenManager.TopBar.SetPlayerHp(_client.PlayerData.CurrentHp / _client.PlayerData.MaxHp);
    }

    private void HandleOnAbilityUsed(int whoId, int abilityId)
    {
        var t = (AbilityType)abilityId;

        if (t == AbilityType.Sword && whoId == _client.EnemyData.Id)
        {
            _client.PlayerData.Abilities.Add(AbilityFactory.CreateAbility(t));
            Logger.Log("add enemy ability:" + t);
        }
        else if (whoId == _client.PlayerData.Id)
        {
            if (t == AbilityType.Axe || t == AbilityType.Spear || t == AbilityType.Mace)
            {
                _client.PlayerData.Abilities.Add(AbilityFactory.CreateAbility(t));
                Logger.Log("add ability:" + t);
            }
        }
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
