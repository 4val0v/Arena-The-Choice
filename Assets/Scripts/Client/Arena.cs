using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Arena : MonoBehaviour {

	void Start ()
	{
	
	}
	
	void Update ()
	{
		if (_time >= 0)
		{
			_time += Time.deltaTime;
			if(_time >= 1)
			{
				_time = 0;
				time--;
				if (time == 0)
				{
					_time = -1;
					time = 3;
					_timerText.gameObject.SetActive (false);
				}
				_timerText.text = ""+time;
			}
		}
	}

	void OnEnable()
	{
		_time = 0;
		_timerText.text = ""+time;
		_timerText.gameObject.SetActive (true);
	}

	void OnDisable()
	{
		_time = -1;
		time = 3;
		_timerText.gameObject.SetActive (false);
	}

	private int time = 3;
	private float _time = -1; 
	[SerializeField]
	private Text _timerText;
}
