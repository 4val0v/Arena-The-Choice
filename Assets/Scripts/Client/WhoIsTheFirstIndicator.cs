﻿using UnityEngine;
using System.Collections;
using System;

public class WhoIsTheFirstIndicator : MonoBehaviour 
{

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}

	public void Play(string name)
	{
		_whoIsTheFirst.Play (name);
	}
	
	private void OnEndAnimChangeFirst()
	{
		if (OnEndAnimChoise != null)
		{
			OnEndAnimChoise.Invoke();
		}
	}

	[SerializeField]
	private Animator _whoIsTheFirst;

	public Action OnEndAnimChoise;
}
