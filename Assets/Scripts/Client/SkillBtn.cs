using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SkillBtn : MonoBehaviour {

	void Start () 
	{
	}
	
	void Update () 
	{
		if (_cdwn > 0)
		{
			_cdwn -= Time.deltaTime;
			_text.text = ""+((int)_cdwn+1);
		}
		else if (_cdwn != -1)
		{
			_cdwn = -1;
			_text.text = "";
			ActivateBtn (true);
		}
	}
	
	public void SetAbillity(AbilityData abbility)
	{
		_abl = abbility;
		_btn.image.sprite = _abl.Icon;
	}

	void OnEnable()
	{
		_text.text = "";
		_cdwn = -1;
	}

	public void StartCouldown()
	{
		ActivateBtn (false);
		_cdwn = _abl.Cooldown;
	}

	public void OnClick()
	{
		if (_abl.Id != AbilityType.Shield)
		{
			StartCouldown ();
		}
		else
		{
			ActivateBtn (false);
		}
		if (OnClickToBtn != null)
		{
			OnClickToBtn.Invoke(_abl);
		}
	}

	public void ActivateBtn(bool isActive)
	{
		_btn.interactable = isActive;
	}

	public AbilityData Abl {
		get {
			return _abl;
		}
	}

	private AbilityData _abl;

	public Action<AbilityData> OnClickToBtn;

	private float _cdwn;

	[SerializeField]
	private Button _btn;

	[SerializeField]
	private Text _text;

}
