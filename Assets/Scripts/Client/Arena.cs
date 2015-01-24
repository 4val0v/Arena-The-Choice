using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Arena : MonoBehaviour {

	void Start ()
	{
	
	}
	
	void Update ()
	{
		FightCounter ();
		ScreenTimeCounter ();
	}

	private void FightCounter()
	{
		if (!_fightStarted)
		{
			return;
		}
		timerOfFight += Time.deltaTime;
		if(timerOfFight >= _timeBeforeNextKick)
		{
			TimeBeforeNextKick();
			timerOfFight = 0;
			_client.SendAttack(0, (int)_client.PlayerData.Dmg); 
		}
	}

	private void ScreenTimeCounter()
	{
		if (_timerOnScreen >= 0)
		{
			_timerOnScreen += Time.deltaTime;
			if(_timerOnScreen >= 1)
			{
				_timerOnScreen = 0;
				time--;
				if (time == 0)
				{
					TimerFinish();
				}
				_timerText.text = ""+time;
			}
		}
	}

	void OnEnable()
	{
		_timerOnScreen = 0;
		_timerText.text = ""+time;
		_timerText.gameObject.SetActive (true);
		ActivateAllBtns (false);
	}

	void OnDisable()
	{
		_timerOnScreen = -1;
		time = 3;
		_timerText.gameObject.SetActive (false);
		_fightStarted = false;
	}

	private void TimerFinish()
	{
		_timerOnScreen = -1;
		time = 3;
		_timerText.gameObject.SetActive (false);
		ActivateAllBtns (true);
		StartFight ();
	}

	private void ActivateAllBtns(bool active)
	{
		foreach (var item in _skillButtons) 
		{
			item.ActivateBtn(active);
		}
	}

	void StartFight ()
	{
		TimeBeforeNextKick ();
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
}
