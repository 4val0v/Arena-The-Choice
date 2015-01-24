using UnityEngine;
using System.Collections;

public class GetItem : MonoBehaviour {

	void Start () 
	{
		_whoIsTheFirst.OnEndAnimChoise += EndFirstPlayerAnim;
	}
	
	void Update () 
	{
	
	}

	public bool IsYouFirst 
	{
		get;
		set;
	}

	void OnEnable()
	{
		if (IsYouFirst)
		{
			_whoIsTheFirst.Play("you");
		}
		else
		{
			_whoIsTheFirst.Play("enemy");
		}
	}

	private void EndFirstPlayerAnim()
	{
		Logger.Log ("End First playerAnim");
		_whoIsTheFirst.gameObject.SetActive (false);
	}


	[SerializeField]
	private WhoIsTheFirstIndicator _whoIsTheFirst;

}
