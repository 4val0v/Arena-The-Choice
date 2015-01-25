using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Arena : MonoBehaviour
{
    public CharacterView MyMan;
    public CharacterView MyWoman;

    public CharacterView EnemyMan;
    public CharacterView EnemyWoman;

    private CharacterView _player;
    private CharacterView _enemy;

    void Awake()
    {
        foreach (var item in _skillButtons)
        {
            item.OnClickToBtn = UseAbbility;
        }

        enemyScores = new Vector2(200, 0);

        playerScores = new Vector2(-200, 0);
    }

    private void UseAbbility(AbilityData data)
    {
        _client.UseAbility((int)data.Id);
    }

    void Update()
    {
        FightCounter();
        ScreenTimeCounter();
    }

    private void FightCounter()
    {
        if (!_fightStarted)
        {
            return;
        }
        timerOfFight += Time.deltaTime;

        if (_client.PlayerData != null)
            for (int i = _client.PlayerData.Abilities.Count - 1; i >= 0; i--)
            {
                var ability = _client.PlayerData.Abilities[i];
                ability.Update();
            }

        if (timerOfFight >= _timeBeforeNextKick)
        {
            if (_client.PlayerData != null)
                for (int i = _client.PlayerData.Abilities.Count - 1; i >= 0; i--)
                {
                    var ability = _client.PlayerData.Abilities[i];
                    ability.UpdateOnAttack();
                }

            TimeBeforeNextKick();
            timerOfFight = 0;
            _dmg = (int)_client.PlayerData.Dmg;

            if (_dmg < 0)
                _dmg = 0;

            if (Random.value > _client.PlayerData.Accuracy)
            {
                _dmg = 0;
            }
            _client.SendAttack(0, _dmg);



            //play anim for second hhand
            foreach (var item in _client.PlayerData.EquippedItems)
            {
                if (item == 5 || item == 6)
                {
                    _player.PlayAttack(false);
                    return;
                }
            }

            //play anim for main hand
            _player.PlayAttack(true);
        }
    }

    private void ScreenTimeCounter()
    {
        if (_timerOnScreen >= 0)
        {
            _timerOnScreen += Time.deltaTime;
            if (_timerOnScreen >= 1)
            {
                _timerOnScreen = 0;
                time--;
                if (time == 0)
                {
                    TimerFinish();
                }
                _timerText.text = "" + time;
            }
        }
    }

    void OnEnable()
    {
        MyMan.HideAll();
        MyWoman.HideAll();
        EnemyMan.HideAll();
        EnemyWoman.HideAll();

        _timerOnScreen = 0;
        _timerText.text = "" + time;
        _timerText.gameObject.SetActive(true);
        ActivateAllBtns(false);
        for (int i = 0; i < 3; i++)
        {
            _skillButtons[i].SetAbillity(ItemsProvider.GetItem(_client.PlayerData.EquippedItems[i]).Ability);
        }

        ShowPlayer();
        ShowEnemy();
    }

    void OnDisable()
    {
        _timerOnScreen = -1;
        time = 3;
        _timerText.gameObject.SetActive(false);
        _fightStarted = false;

        if (_enemy != null)
        {
            _enemy.HideAll();
            _enemy = null;
        }

        if (_player != null)
        {
            _player.HideAll();
            _player = null;
        }
    }

    private void ShowPlayer()
    {
        var data = _client.PlayerData;

        if (data.Class == CharacterClass.Man)
            _player = MyMan;
        else if (data.Class == CharacterClass.Woman)
            _player = MyWoman;

        _player.ShowOnlyIds(data.EquippedItems);
    }

    private void ShowEnemy()
    {
        var data = _client.EnemyData;

        if (data.Class == CharacterClass.Man)
            _enemy = EnemyMan;
        else if (data.Class == CharacterClass.Woman)
            _enemy = EnemyWoman;

        _enemy.ShowOnlyIds(data.EquippedItems);
    }

    private void TimerFinish()
    {
        _timerOnScreen = -1;
        time = 3;
        _timerText.gameObject.SetActive(false);
        ActivateAllBtns(true);
        StartFight();
    }

    private void ActivateAllBtns(bool active)
    {
        foreach (var item in _skillButtons)
        {
            item.ActivateBtn(active);
        }
    }

    void StartFight()
    {
        TimeBeforeNextKick();
        _fightStarted = true;
    }

    void TimeBeforeNextKick()
    {
        _timeBeforeNextKick = 60 / _client.PlayerData.AttackSpeed;
    }

    public INetClient Client
    {
        set
        {
            _client = value;
        }
    }

    public void MakeDmgToPlayer(int dmg)
    {
        dmg = -dmg;
        var score = _scoresPrefab.Spawn(gameObject.transform, playerScores).GetComponent<DamageNums>();
        string scrText = "";
        if (dmg == 0)
        {
            _color = Color.red;
            scrText = Texts.MISS;
        }
        else if (dmg > 0)
        {
            scrText = "+" + dmg;
            _color = Color.green;
        }
        else
        {
            scrText = "" + dmg;
            _color = Color.red;
        }

        score.SetText(scrText, _color);
    }

    public void MakeDmgToEnemy(int dmg)
    {
        dmg = -dmg;
        var score = _scoresPrefab.Spawn(gameObject.transform, enemyScores).GetComponent<DamageNums>();
        string scrText = "";
        if (dmg == 0)
        {
            _color = Color.red;
            scrText = Texts.MISS;
        }
        else if (dmg > 0)
        {
            scrText = "+" + dmg;
            _color = Color.green;
        }
        else
        {
            scrText = "" + dmg;
            _color = Color.red;
        }
        score.SetText(scrText, _color);
    }

    public void PlayEnemyAttack()
    {
        //play anim for second hhand
        foreach (var item in _client.EnemyData.EquippedItems)
        {
            if (item == 5 || item == 6)
            {
                _enemy.PlayAttack(false);
                return;
            }
        }

        //play anim for main hand
        _enemy.PlayAttack(true);
    }

	public void StartCouldownFor(AbilityType type)
	{
		foreach (var item in _skillButtons) 
		{
			if (item.Abl.Id == type)
			{
				item.StartCouldown();
			}
		}
	}

    private Color _color;

    private int _dmg;
    private bool _fightStarted;
    private float timerOfFight;
    private float _timeBeforeNextKick;

    private INetClient _client;

    private int time = 3;
    private float _timerOnScreen = -1;

    [SerializeField]
    private Text _timerText;

    [SerializeField]
    private List<SkillBtn> _skillButtons;

    [SerializeField]
    private GameObject _scoresPrefab;

    private Vector2 enemyScores;
    private Vector2 playerScores;

}
