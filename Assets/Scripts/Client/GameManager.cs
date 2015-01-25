using System;
using System.Linq;
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
        _client.OnHpAdjusted += ClientOnOnHpAdjusted;
        _client.OnEnemyHpAdjusted += ClientOnOnEnemyHpAdjusted;

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
        //Logger.Log("Game started!");
        _screenManager.Arena.Client = _client;
        _screenManager.ChangeScreen(ScreenManager.Screens.Arena);
    }

    void HandleOnItemEquipped(int playerId, int itemId)
    {
        //get item
        //Logger.Log("ItemEquipped: playerId:" + playerId + ", itemId:" + itemId);

        //refresh
        _screenManager.GetItem.UpdateEquippedItems(_client.PlayerData.EquippedItems, _client.EnemyData.EquippedItems);

        _screenManager.GetItem.SetTurn(playerId != _client.PlayerData.Id);

        _screenManager.TopBar.SetDmg(_client.PlayerData.Dmg);
        _screenManager.TopBar.SetDef(_client.PlayerData.Def);
        _screenManager.TopBar.SetAttackSpeed(_client.PlayerData.AttackSpeed);

        _screenManager.TopBar.SetEnemyDmg(_client.EnemyData.Dmg);
        _screenManager.TopBar.SetEnemyDef(_client.EnemyData.Def);
        _screenManager.TopBar.SetEnemyAttackSpeed(_client.EnemyData.AttackSpeed);

        UpdateInventory();
    }

    void HandleOnStepItemsReceived(EquipStep step, System.Collections.Generic.IEnumerable<int> items)
    {
        //create screen change item
        //Logger.Log("Current step:" + step + ", items...");

        _screenManager.GetItem.UpdateStock(step, items);

        var my = _screenManager.GetItem.FirstPlayerId == _client.PlayerData.Id
            ? (step == EquipStep.FirstHand || step == EquipStep.Armor)
            : step == EquipStep.SecondHand;

        _screenManager.GetItem.SetTurn(my);

        _screenManager.GetItem.UpdateSelection();
    }

    void HandleOnFirstPlayerReceived(int playerId)
    {
        //Logger.Log("I'm first:" + (_client.PlayerData.Id == playerId) + ", fisrtId:" + playerId);

        _screenManager.GetItem.SetFirstPlayerId(playerId);
        _screenManager.GetItem.IsYouFirst = _client.PlayerData.Id == playerId;
    }

    void HandleOnEnemyClassUpdated(CharacterClass classId)
    {
        CheckLastSelectionOfCharacter();

        _screenManager.TopBar.SetEnemyDmg(_client.EnemyData.Dmg);
        _screenManager.TopBar.SetEnemyDef(_client.EnemyData.Def);
        _screenManager.TopBar.SetEnemyAttackSpeed(_client.EnemyData.AttackSpeed);

        _client.EnemyData.CurrentHp = _client.EnemyData.MaxHp;
        _screenManager.TopBar.SetEnemyHp(1f, _client.EnemyData.MaxHp);
        _screenManager.TopBar.SetEnemyIcon(CharacterDataProviders.GetBaseData(classId).Icon);
    }

    void HandleOnClassUpdated(CharacterClass classId)
    {
        CheckLastSelectionOfCharacter();

        _screenManager.TopBar.SetDmg(_client.PlayerData.Dmg);
        _screenManager.TopBar.SetDef(_client.PlayerData.Def);
        _screenManager.TopBar.SetAttackSpeed(_client.PlayerData.AttackSpeed);

        _client.PlayerData.CurrentHp = _client.PlayerData.MaxHp;
        _screenManager.TopBar.SetPlayerHp(1f, _client.PlayerData.MaxHp);

        _screenManager.TopBar.SetIcon(CharacterDataProviders.GetBaseData(classId).Icon);
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
        //Logger.Log("HandleOnEnemyDmgReceived:" + dmg);

        //calculate shield
        for (int i = _client.EnemyData.Abilities.Count - 1; i >= 0; i--)
        {
            var ab = _client.EnemyData.Abilities[i];

            ab.UpdateOnDmgReceive();
        }

        _client.EnemyData.CurrentHp -= dmg;
        _screenManager.Arena.MakeDmgToEnemy(dmg);
        _screenManager.TopBar.SetEnemyHp(_client.EnemyData.CurrentHp / _client.EnemyData.MaxHp, _client.EnemyData.CurrentHp);
    }

    private void HandleOnDmgReceived(int weaponId, int dmg)
    {
        //Logger.Log("HandleOnDmgReceived:" + dmg);

        _client.PlayerData.CurrentHp -= dmg;
        _screenManager.Arena.MakeDmgToPlayer(dmg);
        _screenManager.TopBar.SetPlayerHp(_client.PlayerData.CurrentHp / _client.PlayerData.MaxHp, _client.PlayerData.CurrentHp);

        for (int i = _client.PlayerData.Abilities.Count - 1; i >= 0; i--)
        {
            var ability = _client.PlayerData.Abilities[i];
            ability.UpdateOnDmgReceive();
        }

        _screenManager.Arena.PlayEnemyAttack();
    }

    private void HandleOnAbilityUsed(int whoId, int abilityId)
    {
        var t = (AbilityType)abilityId;

        if (whoId == _client.EnemyData.Id)
        {
            if (t == AbilityType.Sword || t == AbilityType.Dagger)
            {
                _client.PlayerData.Abilities.Add(AbilityFactory.CreateAbility(t, this));
                RecalculateAdditionalStats();
                //Logger.Log("add enemy ability:" + t);
            }

            //снимать щит если чел использовал что-то свое
            if (t == AbilityType.Axe || t == AbilityType.Sword || t == AbilityType.Mace || t == AbilityType.Spear)
            {
                if (_client.PlayerData.Abilities.Any(n => n.Id == AbilityType.Shield || n.Id == AbilityType.BigShield))
                {
                    _client.PlayerData.Abilities.RemoveAll(n => n.Id == AbilityType.Shield || n.Id == AbilityType.BigShield);
                    _screenManager.Arena.StartCouldownFor(AbilityType.Shield);
                    _screenManager.Arena.StartCouldownFor(AbilityType.BigShield);
                }
            }

            if (t == AbilityType.Shield || t == AbilityType.BigShield)
            {
                _client.EnemyData.Abilities.Add(AbilityFactory.CreateAbility(AbilityType.EnemyShield, this));
            }
        }
        else if (whoId == _client.PlayerData.Id)
        {
            if (t == AbilityType.Axe || t == AbilityType.Sword || t == AbilityType.Mace || t == AbilityType.Spear)
            {
                if (_client.EnemyData.Abilities.Any(n => n.Id == AbilityType.EnemyShield))
                {
                    _client.EnemyData.Abilities.RemoveAll(n => n.Id == AbilityType.EnemyShield);
                }
            }

            if (t == AbilityType.Axe || t == AbilityType.Spear || t == AbilityType.Mace || t == AbilityType.Shield || t == AbilityType.BigShield || t == AbilityType.Helm)
            {
                _client.PlayerData.Abilities.Add(AbilityFactory.CreateAbility(t, this));

                //if use health, destroy dagger effect
                if (t == AbilityType.Helm)
                {
                    foreach (var ability in _client.PlayerData.Abilities)
                    {
                        if (ability.Id == AbilityType.Dagger)
                        {
                            _client.PlayerData.Abilities.Remove(ability);
                            break;
                        }
                    }
                }

                RecalculateAdditionalStats();
                //Logger.Log("add ability:" + t);
            }
        }
    }

    private void ClientOnOnEnemyHpAdjusted(int adjHp)
    {
        //Logger.Log("ClientOnOnEnemyHpAdjusted:" + adjHp);

        _client.EnemyData.CurrentHp += adjHp;
        _screenManager.Arena.MakeDmgToEnemy(-adjHp);
        _screenManager.TopBar.SetEnemyHp(_client.EnemyData.CurrentHp / _client.EnemyData.MaxHp, _client.EnemyData.CurrentHp);
    }

    private void ClientOnOnHpAdjusted(int adjHp)
    {
        //Logger.Log("ClientOnOnHpAdjusted:" + adjHp);

        _client.PlayerData.CurrentHp += adjHp;
        _screenManager.Arena.MakeDmgToPlayer(-adjHp);
        _screenManager.TopBar.SetPlayerHp(_client.PlayerData.CurrentHp / _client.PlayerData.MaxHp, _client.PlayerData.CurrentHp);
    }

    private void UpdateInventory()
    {
        var sprites = new Sprite[3] { null, null, null };

        //refresh my inventory
        int i = 0;
        foreach (var item in _client.PlayerData.EquippedItems)
        {
            sprites[i++] = ItemsProvider.GetItem(item).Icon;
        }
        _screenManager.TopBar.SetItemsIconsLeft(sprites[0], sprites[1], sprites[2]);

        //refresh enemy inventory
        sprites = new Sprite[3] { null, null, null };
        i = 0;
        foreach (var item in _client.EnemyData.EquippedItems)
        {
            sprites[i++] = ItemsProvider.GetItem(item).Icon;
        }
        _screenManager.TopBar.SetItemsIconsRight(sprites[0], sprites[1], sprites[2]);
    }

    public void RecalculateAdditionalStats()
    {
        float addDmg = 0f;
        float addDef = 0f;
        float addAS = 0f;

        foreach (var ability in _client.PlayerData.Abilities)
        {
            addDmg += ability.Dmg;
            addDef += ability.Def;
            addAS += ability.AttackSpeed;
        }

        _screenManager.TopBar.SetDmg(_client.PlayerData.DmgWithoutAbilities, addDmg);
        _screenManager.TopBar.SetDef(_client.PlayerData.Def, addDef);
        _screenManager.TopBar.SetAttackSpeed(_client.PlayerData.AttackSpeed - addAS, addAS);
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
