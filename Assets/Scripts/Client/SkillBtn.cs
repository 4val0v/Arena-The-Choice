using UnityEngine;
using System.Collections;

public class SkillBtn : MonoBehaviour {

	void Start () 
	{
	}
	
	void Update () 
	{
	
	}
	
	public void SetAbillity(AbilityData abbility)
	{
		_abl = abbility;
	}

	private AbilityData _abl;

}
