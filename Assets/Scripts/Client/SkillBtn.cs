using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	}

	void OnEnable()
	{
		_text.text = "";
		_cdwn = -1;
	}

	void StartCouldown()
	{
		ActivateBtn (false);
		_cdwn = _abl.Cooldown;
	}

	public void ActivateBtn(bool isActive)
	{
		_btn.interactable = isActive;
	}

	private AbilityData _abl;

	private float _cdwn;

	[SerializeField]
	private Button _btn;

	[SerializeField]
	private Text _text;

}
